using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // POST: api/order
    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDto dto)
    {
        var response = await _orderService.CreateOrderAsync(dto);
        if (!response.IsSuccess)
            return BadRequest(response.Message);

        return Ok(response.Data);
    }

    // GET: api/order/employee/{employeeId}
    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetOrdersByEmployeeAsync(int employeeId)
    {
        var response = await _orderService.GetOrdersByEmployeeAsync(employeeId);
        if (!response.IsSuccess)
            return NotFound(response.Message);

        return Ok(response.Data);
    }

    // GET: api/order/chef/{chefId}
    [HttpGet("chef/{chefId}")]
    public async Task<IActionResult> GetOrdersForChefAsync(int chefId)
    {
        var response = await _orderService.GetOrdersForChefAsync(chefId);
        if (!response.IsSuccess)
            return NotFound(response.Message);

        return Ok(response.Data);
    }

    // GET: api/order/company/{companyId}
    [HttpGet("company/{companyId}")]
    public async Task<IActionResult> GetOrdersByCompanyAsync(int companyId)
    {
        var response = await _orderService.GetOrdersByCompanyAsync(companyId);
        if (!response.IsSuccess)
            return NotFound(response.Message);

        return Ok(response.Data);
    }

    // GET: api/order/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAllOrdersAsync()
    {
        var response = await _orderService.GetAllOrdersAsync();
        if (!response.IsSuccess)
            return NotFound(response.Message);

        return Ok(response.Data);
    }

    // GET: api/order/{orderId}
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderByIdAsync(int orderId)
    {
        var response = await _orderService.GetOrderByIdAsync(orderId);
        if (!response.IsSuccess)
            return NotFound(response.Message);

        return Ok(response.Data);
    }

    [HttpPatch("chef/update-status")]
    [Authorize(Roles = "Chef")]
    public async Task<IActionResult> UpdateOrderStatusByChef([FromBody] UpdateOrderStatusByChefDto dto)
    {
        var response = await _orderService.UpdateOrderStatusByChefAsync(dto.OrderId, dto.NewStatus, dto.ChefId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("track/{trackingNumber}")]
    public async Task<IActionResult> TrackOrderByTrackingNumberAsync(string trackingNumber)
    {
        var response = await _orderService.TrackOrderByTrackingNumberAsync(trackingNumber);
        if (!response.IsSuccess)
            return NotFound(response.Message);

        return Ok(response.Data);
    }
}
