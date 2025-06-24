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
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        IEmployeeRepository employeeRepository,
        IMealRepository mealRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _employeeRepository = employeeRepository;
        _mealRepository = mealRepository;
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

        if (newStatus != "ReadyForPickup")
            return GenericResponse<bool>.Fail("Only 'ReadyForPickup' status can be set by chefs.");

        order.DeliveryStatus = DeliveryStatus.ReadyForPickup;
        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync();

        return GenericResponse<bool>.Success(true, "Order status updated to Ready for Pickup.");
    }
}
