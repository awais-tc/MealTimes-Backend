using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;
namespace MealTimes.Core.Service;

public interface IMealService
{
    Task<GenericResponse<MealDto>> CreateMealAsync(MealCreateDto dto);
    Task<GenericResponse<MealDto>> UpdateMealAsync(MealUpdateDto dto);
    Task<GenericResponse<bool>> DeleteMealAsync(int mealId);
    Task<GenericResponse<MealDto>> GetMealByIdAsync(int id);
    Task<GenericResponse<IEnumerable<MealDto>>> GetAllMealsAsync();
    Task<GenericResponse<IEnumerable<MealDto>>> GetMealsByChefAsync(int chefId);
    Task<GenericResponse<IEnumerable<MealDto>>> FilterMealsAsync(MealFilterDto filter);
    Task<GenericResponse<IEnumerable<MealDto>>> GetTopRatedMealsAsync(int count = 5);
}

