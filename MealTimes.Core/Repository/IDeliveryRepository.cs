using MealTimes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.Repository
{
    public interface IDeliveryRepository
    {
        Task<Delivery?> GetByIdAsync(int id);
        Task<IEnumerable<Delivery>> GetAllAsync();
        Task<IEnumerable<Delivery>> GetByDeliveryPersonIdAsync(int deliveryPersonId);
        Task AddAsync(Delivery delivery);
        Task UpdateAsync(Delivery delivery);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
