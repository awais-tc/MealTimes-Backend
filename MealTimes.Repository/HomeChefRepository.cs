using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace MealTimes.Repository
{
    public class HomeChefRepository : IHomeChefRepository
    {
        private readonly AppDbContext _context;

        public HomeChefRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HomeChef?> GetByUserIdAsync(int userId)
        {
            return await _context.HomeChefs.FirstOrDefaultAsync(c => c.UserID == userId);
        }

        public async Task<HomeChef?> GetByIdAsync(int chefId)
        {
            return await _context.HomeChefs
                .Include(h => h.User)
                .Include(h => h.Meals)
                .Include(h => h.Orders)
                .FirstOrDefaultAsync(c => c.ChefID == chefId);
        }

        public async Task DeleteAsync(int id)
        {
            var chef = await GetByUserIdAsync(id);
            _context.HomeChefs.Remove(chef);
            await Task.CompletedTask; // removal is tracked; actual deletion occurs on SaveChangesAsync
        }
    }
}
