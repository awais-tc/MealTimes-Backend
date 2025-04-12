using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Repository;
using Microsoft.EntityFrameworkCore;

namespace MealTimes.Repository;
public class MealRepository : IMealRepository
{
    private readonly AppDbContext _context;

    public MealRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Meal> GetMealByIdAsync(int id)
    {
        return await _context.Meals
            .Include(m => m.Chef).ThenInclude(c => c.User)
            .FirstOrDefaultAsync(m => m.MealID == id);
    }

    public async Task<IEnumerable<Meal>> GetAllMealsAsync()
    {
        return await _context.Meals
            .Include(m => m.Chef).ThenInclude(c => c.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Meal>> GetMealsByChefAsync(int chefId)
    {
        return await _context.Meals
            .Include(m => m.Chef).ThenInclude(c => c.User)
            .Where(m => m.ChefID == chefId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Meal>> FilterMealsAsync(MealFilterDto filter)
    {
        var query = _context.Meals
            .Include(m => m.Chef).ThenInclude(c => c.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Category))
            query = query.Where(m => m.MealCategory == filter.Category);

        if (filter.MinPrice.HasValue)
            query = query.Where(m => m.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(m => m.Price <= filter.MaxPrice.Value);

        if (filter.AvailableOnly == true)
            query = query.Where(m => m.Availability);

        // DietaryPreference is pending future implementation (e.g. tagging system)

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Meal>> GetTopRatedMealsAsync(int count = 5)
    {
        return await _context.Meals
            .Include(m => m.Chef).ThenInclude(c => c.User)
            .OrderByDescending(m => m.Rating)
            .Take(count)
            .ToListAsync();
    }

    public async Task AddMealAsync(Meal meal)
    {
        await _context.Meals.AddAsync(meal);
    }

    public async Task UpdateMealAsync(Meal meal)
    {
        _context.Meals.Update(meal);
    }

    public async Task DeleteMealAsync(Meal meal)
    {
        _context.Meals.Remove(meal);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
