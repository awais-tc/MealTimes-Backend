using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;

namespace MealTimes.Core.Service
{
    public interface IHomeChefService
    {
        Task<GenericResponse<HomeChefDto>> GetByIdAsync(int chefId);
    }
}
