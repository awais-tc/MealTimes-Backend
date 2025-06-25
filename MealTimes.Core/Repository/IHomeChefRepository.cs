using MealTimes.Core.Models;

namespace MealTimes.Core.Repository
{
    public interface IHomeChefRepository
    {
        Task<HomeChef?> GetByUserIdAsync(int userId);
        Task<HomeChef?> GetByIdAsync(int chefId);
        Task DeleteAsync(int id);
    }
}
