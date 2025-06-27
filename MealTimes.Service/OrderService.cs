using AutoMapper;
using MealTimes.Core.DTOs;
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
        // Get Employee and Company
        var employee = await _employeeRepository.GetEmployeeWithCompanyAsync(dto.EmployeeID);
        if (employee == null)
            return GenericResponse<OrderResponseDto>.Fail("Employee not found");

        var company = employee.CorporateCompany;
        if (company == null || company.ActiveSubscriptionPlan == null)
        {
            return GenericResponse<OrderResponseDto>.Fail("Company does not have an active subscription plan");
        }

        if (!company.PlanStartDate.HasValue || !company.PlanEndDate.HasValue || company.PlanEndDate < DateTime.UtcNow)
        {
            company.ActiveSubscriptionPlanID = null;
            company.PlanStartDate = null;
            company.PlanEndDate = null;
            await _corporateCompanyRepository.UpdateAsync(company);

            return GenericResponse<OrderResponseDto>.Fail("The subscription plan has expired and has been deactivated.");
        }

        var today = DateTime.UtcNow.Date;
        var existingOrders = await _orderRepository.GetOrdersByEmployeeAsync(dto.EmployeeID);
        var todaysOrders = existingOrders
            .Where(o => o.OrderDate.Date == today)
            .SelectMany(o => o.OrderMeals)
            .ToList();

        // Validate daily meal limit
        int maxAllowed = company.ActiveSubscriptionPlan.MealLimitPerDay;
        int mealsRequested = dto.Meals.Count;
        if ((todaysOrders.Count + mealsRequested) > maxAllowed)
            return GenericResponse<OrderResponseDto>.Fail($"Daily meal limit exceeded. You can order up to {maxAllowed} meals per day.");

        // Validate for duplicate meals
        foreach (var meal in dto.Meals)
        {
            if (todaysOrders.Any(m => m.MealID == meal.MealID))
                return GenericResponse<OrderResponseDto>.Fail($"Meal with ID {meal.MealID} already ordered today");
        }

        // Auto-assign chef (based on first meal, assuming all meals from the same chef)
        var firstMeal = await _mealRepository.GetMealByIdAsync(dto.Meals.First().MealID);
        if (firstMeal == null)
            return GenericResponse<OrderResponseDto>.Fail("Meal not found");

        int chefId = firstMeal.ChefID;

        // Map DTO to Order entity
        var newOrder = new Order
        {
            EmployeeID = dto.EmployeeID,
            ChefID = chefId,
            OrderDate = DateTime.UtcNow,
            DeliveryStatus = DeliveryStatus.Pending, // Enum mapped to string
            PaymentStatus = PaymentStatus.Succeeded, // Enum mapped to string
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
}
