using MealTimes.Core.DTOs;
using MealTimes.Core.Models;

namespace MealTimes.Core.Repository;
public interface IMealRepository
{
    Task<Meal> GetMealByIdAsync(int id);
    Task<IEnumerable<Meal>> GetAllMealsAsync();
    Task<IEnumerable<Meal>> GetMealsByChefAsync(int chefId);
    Task<IEnumerable<Meal>> FilterMealsAsync(MealFilterDto filter);
    Task<IEnumerable<Meal>> GetTopRatedMealsAsync(int count = 5);
    Task AddMealAsync(Meal meal);
    Task UpdateMealAsync(Meal meal);
    Task DeleteMealAsync(Meal meal);
    Task SaveChangesAsync();
}

