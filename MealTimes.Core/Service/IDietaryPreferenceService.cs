using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;

namespace MealTimes.Core.Service
{
    public interface IDietaryPreferenceService
    {
        Task<GenericResponse<DietaryPreferenceDto>> GetByEmployeeIdAsync(int employeeId);
        Task<GenericResponse<string>> UpsertAsync(int employeeId, DietaryPreferenceDto dto);
    }
}
