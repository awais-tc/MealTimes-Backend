using MealTimes.Core.Models;
using MealTimes.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MealTimes.Core.Repository
{
    public class SubscriptionHistoryRepository : ISubscriptionHistoryRepository
    {
        private readonly AppDbContext _context;

        public SubscriptionHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CompanySubscriptionHistory>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.CompanySubscriptionHistories
                .Where(sh => sh.CorporateCompanyId == companyId)
                .ToListAsync();
        }

        public async Task AddAsync(CompanySubscriptionHistory subscriptionHistory)
        {
            await _context.CompanySubscriptionHistories.AddAsync(subscriptionHistory);
            await _context.SaveChangesAsync();
        }
    }
}
