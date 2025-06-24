using MealTimes.Core.Models;

namespace MealTimes.Core.Repository
{
    public interface IDeliveryPersonRepository
    {
        Task<DeliveryPerson?> GetByUserIdAsync(int userId);
        Task<DeliveryPerson?> GetByIdAsync(int id);
        Task<IEnumerable<DeliveryPerson>> GetAllAsync();
        Task AddAsync(DeliveryPerson deliveryPerson);
        Task UpdateAsync(DeliveryPerson deliveryPerson);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
