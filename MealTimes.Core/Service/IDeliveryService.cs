using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;

namespace MealTimes.Core.Service
{
    public interface IDeliveryService
    {
        Task<GenericResponse<DeliveryDto>> AssignDeliveryAsync(DeliveryAssignDto dto);
        Task<GenericResponse<bool>> UpdateDeliveryStatusAsync(DeliveryStatusUpdateDto dto);
        Task<GenericResponse<IEnumerable<DeliveryDto>>> GetAllDeliveriesAsync();
        Task<GenericResponse<DeliveryDto>> GetDeliveryByIdAsync(int id);
        Task<GenericResponse<IEnumerable<DeliveryDto>>> GetDeliveriesByPersonIdAsync(int deliveryPersonId);
    }
}
