using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace MealTimes.Repository
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly AppDbContext _context;

        public BusinessRepository(AppDbContext context)
        {
            _context = context;
        }

        // Commission Repository Methods
        public async Task<Commission> CreateCommissionAsync(Commission commission)
        {
            await _context.Commissions.AddAsync(commission);
            await _context.SaveChangesAsync();
            return commission;
        }

        public async Task<Commission?> GetCommissionByOrderIdAsync(int orderId)
        {
            return await _context.Commissions
                .Include(c => c.Chef)
                .Include(c => c.Order)
                .FirstOrDefaultAsync(c => c.OrderID == orderId);
        }

        public async Task<List<Commission>> GetCommissionsByChefAsync(int chefId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Commissions
                .Include(c => c.Chef)
                .Include(c => c.Order)
                .Where(c => c.ChefID == chefId);

            if (startDate.HasValue)
                query = query.Where(c => c.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.CreatedAt <= endDate.Value);

            return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
        }

        public async Task<List<Commission>> GetAllCommissionsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Commissions
                .Include(c => c.Chef)
                .Include(c => c.Order)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(c => c.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.CreatedAt <= endDate.Value);

            return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
        }

        public async Task<Commission> UpdateCommissionAsync(Commission commission)
        {
            _context.Commissions.Update(commission);
            await _context.SaveChangesAsync();
            return commission;
        }

        // Chef Payout Repository Methods
        public async Task<ChefPayout> CreateChefPayoutAsync(ChefPayout payout)
        {
            await _context.ChefPayouts.AddAsync(payout);
            await _context.SaveChangesAsync();
            return payout;
        }

        public async Task<ChefPayout?> GetChefPayoutByIdAsync(int payoutId)
        {
            return await _context.ChefPayouts
                .Include(p => p.Chef)
                .Include(p => p.Commissions)
                .FirstOrDefaultAsync(p => p.PayoutID == payoutId);
        }

        public async Task<List<ChefPayout>> GetChefPayoutsAsync(int? chefId = null, string? status = null)
        {
            var query = _context.ChefPayouts
                .Include(p => p.Chef)
                .AsQueryable();

            if (chefId.HasValue)
                query = query.Where(p => p.ChefID == chefId.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(p => p.Status == status);

            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<ChefPayout> UpdateChefPayoutAsync(ChefPayout payout)
        {
            _context.ChefPayouts.Update(payout);
            await _context.SaveChangesAsync();
            return payout;
        }

        public async Task<List<ChefPayout>> GetPendingPayoutsAsync()
        {
            return await _context.ChefPayouts
                .Include(p => p.Chef)
                .Where(p => p.Status == "Pending")
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> HasPendingPayoutForPeriodAsync(int chefId, DateTime startDate, DateTime endDate)
        {
            return await _context.ChefPayouts
                .AnyAsync(p => p.ChefID == chefId && 
                              p.PeriodStart == startDate && 
                              p.PeriodEnd == endDate &&
                              p.Status != "Failed");
        }

        // Business Metrics Repository Methods
        public async Task<BusinessMetrics> CreateBusinessMetricsAsync(BusinessMetrics metrics)
        {
            await _context.BusinessMetrics.AddAsync(metrics);
            await _context.SaveChangesAsync();
            return metrics;
        }

        public async Task<BusinessMetrics?> GetBusinessMetricsByDateAsync(DateTime date)
        {
            return await _context.BusinessMetrics
                .FirstOrDefaultAsync(m => m.Date.Date == date.Date);
        }

        public async Task<List<BusinessMetrics>> GetBusinessMetricsRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.BusinessMetrics
                .Where(m => m.Date >= startDate && m.Date <= endDate)
                .OrderBy(m => m.Date)
                .ToListAsync();
        }

        public async Task<BusinessMetrics> UpdateBusinessMetricsAsync(BusinessMetrics metrics)
        {
            _context.BusinessMetrics.Update(metrics);
            await _context.SaveChangesAsync();
            return metrics;
        }

        // Analytics Query Methods
        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var subscriptionRevenue = await GetTotalSubscriptionRevenueAsync(startDate, endDate);
            var commissionRevenue = await GetTotalCommissionRevenueAsync(startDate, endDate);
            return subscriptionRevenue + commissionRevenue;
        }

        public async Task<decimal> GetTotalCommissionRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Commissions.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(c => c.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.CreatedAt <= endDate.Value);

            return await query.SumAsync(c => c.PlatformEarning);
        }

        public async Task<decimal> GetTotalSubscriptionRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Payments
                .Where(p => p.SubscriptionPlanID != null && p.PaymentStatus == "succeeded");

            if (startDate.HasValue)
                query = query.Where(p => p.PaymentDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.PaymentDate <= endDate.Value);

            return await query.SumAsync(p => p.PaymentAmount);
        }

        public async Task<decimal> GetTotalChefPayoutsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.ChefPayouts
                .Where(p => p.Status == "Completed");

            if (startDate.HasValue)
                query = query.Where(p => p.ProcessedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.ProcessedAt <= endDate.Value);

            return await query.SumAsync(p => p.PayableAmount);
        }

        public async Task<int> GetTotalOrdersAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Orders.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(o => o.OrderDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(o => o.OrderDate <= endDate.Value);

            return await query.CountAsync();
        }

        public async Task<decimal> GetAverageOrderValueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Orders
                .Include(o => o.OrderMeals)
                .ThenInclude(om => om.Meal)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(o => o.OrderDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(o => o.OrderDate <= endDate.Value);

            var orders = await query.ToListAsync();
            
            if (!orders.Any()) return 0;

            var totalValue = orders.Sum(o => o.OrderMeals.Sum(om => om.Meal.Price * om.Quantity));
            return totalValue / orders.Count;
        }

        public async Task<int> GetActiveSubscriptionsCountAsync()
        {
            return await _context.CorporateCompanies
                .Where(c => c.ActiveSubscriptionPlanID != null && 
                           c.PlanEndDate > DateTime.UtcNow)
                .CountAsync();
        }

        public async Task<int> GetActiveChefsCountAsync()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            return await _context.HomeChefs
                .Where(c => c.Orders.Any(o => o.OrderDate >= thirtyDaysAgo))
                .CountAsync();
        }

        public async Task<int> GetActiveEmployeesCountAsync()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            return await _context.Employees
                .Where(e => e.Orders.Any(o => o.OrderDate >= thirtyDaysAgo))
                .CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}