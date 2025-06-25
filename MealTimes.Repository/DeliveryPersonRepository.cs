using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace MealTimes.Repository
{
    public class DeliveryPersonRepository : IDeliveryPersonRepository
    {
        private readonly AppDbContext _context;

        public DeliveryPersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeliveryPerson?> GetByUserIdAsync(int userId)
        {
            return await _context.DeliveryPersons.FirstOrDefaultAsync(d => d.UserID == userId);
        }

        public async Task<DeliveryPerson?> GetByIdAsync(int id)
        {
            return await _context.DeliveryPersons.FindAsync(id);
        }

        public async Task<IEnumerable<DeliveryPerson>> GetAllAsync()
        {
            return await _context.DeliveryPersons.ToListAsync();
        }

        public async Task AddAsync(DeliveryPerson deliveryPerson)
        {
            await _context.DeliveryPersons.AddAsync(deliveryPerson);
        }

        public Task UpdateAsync(DeliveryPerson deliveryPerson)
        {
            _context.DeliveryPersons.Update(deliveryPerson);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                _context.DeliveryPersons.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
