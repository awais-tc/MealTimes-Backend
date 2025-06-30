using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.Service
{
    public interface IOrderService
    {
        Task<GenericResponse<OrderResponseDto>> CreateOrderAsync(CreateOrderDto dto);
        Task<GenericResponse<List<OrderResponseDto>>> GetOrdersByEmployeeAsync(int employeeId);
        Task<GenericResponse<OrderTrackingDto>> TrackOrderByTrackingNumberAsync(string trackingNumber);
        Task<GenericResponse<List<OrderResponseDto>>> GetOrdersForChefAsync(int chefId);
        Task<GenericResponse<List<OrderResponseDto>>> GetOrdersByCompanyAsync(int companyId);
        Task<GenericResponse<List<OrderResponseDto>>> GetAllOrdersAsync();
        Task<GenericResponse<OrderResponseDto>> GetOrderByIdAsync(int orderId);
        Task<GenericResponse<bool>> UpdateOrderStatusByChefAsync(int orderId, string newStatus, int chefId);
        Task<GenericResponse<string>> CancelOrderAsync(int orderId);
    }
}
