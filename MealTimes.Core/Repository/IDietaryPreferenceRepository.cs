using MealTimes.Core.DTOs;
using MealTimes.Core.Models;

namespace MealTimes.Core.Repository
{
    public interface IDietaryPreferenceRepository
    {
        Task<DietaryPreference?> GetByEmployeeIdAsync(int employeeId);
        Task<DietaryPreference> UpsertAsync(int employeeId, DietaryPreferenceDto dto);
    }
}
