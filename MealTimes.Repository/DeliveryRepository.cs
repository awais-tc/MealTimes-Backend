using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Repository
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly AppDbContext _context;

        public DeliveryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Delivery?> GetByIdAsync(int id)
        {
            return await _context.Deliveries
                .Include(d => d.DeliveryPerson)
                .Include(d => d.Order)
                .FirstOrDefaultAsync(d => d.DeliveryID == id);
        }

        public async Task<Delivery?> GetByTrackingNumberAsync(string trackingNumber)
        {
            return await _context.Deliveries
                .FirstOrDefaultAsync(d => d.TrackingNumber == trackingNumber);
        }

        public async Task<IEnumerable<Delivery>> GetByDeliveryPersonIdAsync(int deliveryPersonId)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.DeliveryPersonID == deliveryPersonId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetAllAsync()
        {
            return await _context.Deliveries
                .Include(d => d.DeliveryPerson)
                .Include(d => d.Order)
                .ToListAsync();
        }

        public async Task AddAsync(Delivery delivery)
        {
            await _context.Deliveries.AddAsync(delivery);
        }

        public async Task UpdateAsync(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
        }

        public async Task DeleteAsync(int id)
        {
            var delivery = await GetByIdAsync(id);
            if (delivery != null)
                _context.Deliveries.Remove(delivery);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
