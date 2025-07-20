using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Helpers;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMealRepository _mealRepository;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ICorporateCompanyRepository _corporateCompanyRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        IEmployeeRepository employeeRepository,
        IMealRepository mealRepository,
        IDeliveryRepository deliveryRepo,
        ICorporateCompanyRepository companyRepo,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _employeeRepository = employeeRepository;
        _mealRepository = mealRepository;
        _deliveryRepository = deliveryRepo;
        _corporateCompanyRepository = companyRepo;
        _mapper = mapper;
    }

    public async Task<GenericResponse<OrderResponseDto>> CreateOrderAsync(CreateOrderDto dto)
    {
        // Step 1: Validate Employee and Company Subscription
        var employee = await _employeeRepository.GetEmployeeWithCompanyAsync(dto.EmployeeID);
        if (employee == null)
            return GenericResponse<OrderResponseDto>.Fail("Employee not found");

        var company = employee.CorporateCompany;
        if (company == null || company.ActiveSubscriptionPlan == null)
            return GenericResponse<OrderResponseDto>.Fail("Company does not have an active subscription plan");

        if (!company.PlanStartDate.HasValue || !company.PlanEndDate.HasValue || company.PlanEndDate < DateTime.UtcNow)
        {
            company.ActiveSubscriptionPlanID = null;
            company.PlanStartDate = null;
            company.PlanEndDate = null;
            await _corporateCompanyRepository.UpdateAsync(company);

            return GenericResponse<OrderResponseDto>.Fail("The subscription plan has expired and has been deactivated.");
        }

        // Step 2: Fetch Employee's existing orders for today
        var today = DateTime.UtcNow.Date;
        var existingOrders = await _orderRepository.GetOrdersByEmployeeAsync(dto.EmployeeID);

        var todaysOrders = existingOrders
            .Where(o => o.OrderDate.Date == today)
            .SelectMany(o => o.OrderMeals)
            .ToList();

        // Step 3: Validate meal limit per employee
        int maxMealsPerDay = company.ActiveSubscriptionPlan.MealLimitPerDay;
        int requestedMealCount = dto.Meals.Count;

        if ((todaysOrders.Count + requestedMealCount) > maxMealsPerDay)
            return GenericResponse<OrderResponseDto>.Fail($"Daily meal limit exceeded. You can order up to {maxMealsPerDay} meals per day.");

        // Step 4: Validate no duplicate meals today
        foreach (var meal in dto.Meals)
        {
            if (todaysOrders.Any(m => m.MealID == meal.MealID))
                return GenericResponse<OrderResponseDto>.Fail($"Meal with ID {meal.MealID} already ordered today.");
        }

        // Step 5: Validate daily employee count limit
        var companyEmployeeIds = company.Employees.Select(e => e.EmployeeID).ToList();
        var todaysEmployeeOrders = existingOrders
            .Where(o => o.OrderDate.Date == today && companyEmployeeIds.Contains(o.EmployeeID))
            .GroupBy(o => o.EmployeeID)
            .Select(g => g.Key)
            .ToList();

        bool isAlreadyCounted = todaysEmployeeOrders.Contains(dto.EmployeeID);
        int currentEmployeeCount = todaysEmployeeOrders.Count;

        int maxEmployeesPerDay = company.ActiveSubscriptionPlan.MaxEmployees;

        if (!isAlreadyCounted && currentEmployeeCount >= maxEmployeesPerDay)
        {
            return GenericResponse<OrderResponseDto>.Fail(
                $"Daily employee limit reached. Only {maxEmployeesPerDay} employees can place orders today."
            );
        }

        // Step 6: Validate all meals belong to same chef
        var meals = await _mealRepository.GetMealsByIdsAsync(dto.Meals.Select(m => m.MealID).ToList());

        if (meals.Count != dto.Meals.Count)
            return GenericResponse<OrderResponseDto>.Fail("One or more meals not found.");

        // Step 6.5: Check delivery distance if specified
        if (dto.MaxDeliveryDistanceKm.HasValue)
        {
            var chef = meals.First().Chef;
            if (chef.Location != null && employee.Location != null)
            {
                var distance = LocationHelper.CalculateDistance(
                    chef.Location.Latitude, chef.Location.Longitude,
                    employee.Location.Latitude, employee.Location.Longitude);

                if (distance > dto.MaxDeliveryDistanceKm.Value)
                {
                    return GenericResponse<OrderResponseDto>.Fail(
                        $"Chef is too far away. Distance: {distance:F2}km, Maximum allowed: {dto.MaxDeliveryDistanceKm.Value}km");
                }
            }
        }

        var distinctChefIds = meals.Select(m => m.ChefID).Distinct().ToList();
        if (distinctChefIds.Count > 1)
            return GenericResponse<OrderResponseDto>.Fail("All meals in a single order must be from the same HomeChef.");

        int chefId = distinctChefIds.First();

        // Step 7: Create Order
        var newOrder = new Order
        {
            EmployeeID = dto.EmployeeID,
            ChefID = chefId,
            OrderDate = DateTime.UtcNow,
            DeliveryStatus = DeliveryStatus.Pending,
            PaymentStatus = PaymentStatus.Succeeded,
            OrderMeals = dto.Meals.Select(m => new OrderMeal
            {
                MealID = m.MealID,
                Quantity = 1
            }).ToList()
        };

        var savedOrder = await _orderRepository.CreateOrderAsync(newOrder);
        var orderDto = _mapper.Map<OrderResponseDto>(savedOrder);

        return GenericResponse<OrderResponseDto>.Success(orderDto, "Order placed successfully");
    }

    public async Task<GenericResponse<List<OrderResponseDto>>> GetOrdersByEmployeeAsync(int employeeId)
    {
        var orders = await _orderRepository.GetOrdersByEmployeeAsync(employeeId);
        var orderDtos = _mapper.Map<List<OrderResponseDto>>(orders);
        return GenericResponse<List<OrderResponseDto>>.Success(orderDtos);
    }

    public async Task<GenericResponse<OrderTrackingDto>> TrackOrderByTrackingNumberAsync(string trackingNumber)
    {
        var delivery = await _deliveryRepository.GetByTrackingNumberAsync(trackingNumber);

        if (delivery == null)
            return GenericResponse<OrderTrackingDto>.Fail("Tracking number not found.");

        var dto = _mapper.Map<OrderTrackingDto>(delivery);

        return GenericResponse<OrderTrackingDto>.Success(dto);
    }

    public async Task<GenericResponse<List<OrderResponseDto>>> GetOrdersForChefAsync(int chefId)
    {
        var orders = await _orderRepository.GetOrdersForChefAsync(chefId);
        var orderDtos = _mapper.Map<List<OrderResponseDto>>(orders);
        return GenericResponse<List<OrderResponseDto>>.Success(orderDtos);
    }

    public async Task<GenericResponse<List<OrderResponseDto>>> GetOrdersByCompanyAsync(int companyId)
    {
        var orders = await _orderRepository.GetOrdersByCompanyAsync(companyId);
        var orderDtos = _mapper.Map<List<OrderResponseDto>>(orders);
        return GenericResponse<List<OrderResponseDto>>.Success(orderDtos);
    }

    public async Task<GenericResponse<List<OrderResponseDto>>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();
        var orderDtos = _mapper.Map<List<OrderResponseDto>>(orders);
        return GenericResponse<List<OrderResponseDto>>.Success(orderDtos);
    }

    public async Task<GenericResponse<OrderResponseDto>> GetOrderByIdAsync(int orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            return GenericResponse<OrderResponseDto>.Fail("Order not found");

        var orderDto = _mapper.Map<OrderResponseDto>(order);
        return GenericResponse<OrderResponseDto>.Success(orderDto);
    }

    public async Task<GenericResponse<bool>> UpdateOrderStatusByChefAsync(int orderId, string newStatus, int chefId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            return GenericResponse<bool>.Fail("Order not found.");

        if (order.ChefID != chefId)
            return GenericResponse<bool>.Fail("Unauthorized: This order is not assigned to you.");

        // Ensure the status is a valid enum value
        if (!Enum.TryParse<DeliveryStatus>(newStatus, out var requestedStatus))
            return GenericResponse<bool>.Fail("Invalid status.");

        // Check valid status transitions
        if (order.DeliveryStatus == DeliveryStatus.Pending && requestedStatus == DeliveryStatus.Preparing)
        {
            order.DeliveryStatus = DeliveryStatus.Preparing;
        }
        else if (order.DeliveryStatus == DeliveryStatus.Preparing && requestedStatus == DeliveryStatus.ReadyForPickup)
        {
            order.DeliveryStatus = DeliveryStatus.ReadyForPickup;
        }
        else
        {
            return GenericResponse<bool>.Fail($"Invalid status transition from {order.DeliveryStatus} to {requestedStatus}.");
        }

        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync();

        return GenericResponse<bool>.Success(true, $"Order status updated to {requestedStatus}.");
    }

    public async Task<GenericResponse<string>> CancelOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            return GenericResponse<string>.Fail("Order not found.");

        // Only allow cancellation if order is still Pending
        if (order.DeliveryStatus != DeliveryStatus.Pending)
            return GenericResponse<string>.Fail("Order cannot be canceled as it is already being prepared or delivered.");

        var timeElapsed = DateTime.UtcNow - order.OrderDate;
        if (timeElapsed.TotalMinutes > 5)
            return GenericResponse<string>.Fail("Cannot cancel the order after 5 minutes of placing.");

        await _orderRepository.DeleteAsync(order); // or implement a soft delete if needed
        await _orderRepository.SaveChangesAsync(); // if using UnitOfWork pattern

        return GenericResponse<string>.Success("Order canceled successfully.");
    }
}