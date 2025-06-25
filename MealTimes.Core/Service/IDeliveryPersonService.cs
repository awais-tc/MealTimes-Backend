using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;

namespace MealTimes.Core.Service
{
    public interface IDeliveryPersonService
    {
        Task<GenericResponse<IEnumerable<DeliveryPersonDto>>> GetAllAsync();
        Task<GenericResponse<DeliveryPersonDto>> GetByIdAsync(int id);
        Task<GenericResponse<DeliveryPersonDto>> UpdateAsync(DeliveryPersonUpdateDto dto);
    }
}
