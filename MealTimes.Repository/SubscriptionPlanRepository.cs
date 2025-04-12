using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Repository;
using Microsoft.EntityFrameworkCore;

public class SubscriptionPlanRepository : ISubscriptionPlanRepository
{
    private readonly AppDbContext _context;

    public SubscriptionPlanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SubscriptionPlan>> GetAllPlansAsync()
    {
        return await _context.SubscriptionPlans.ToListAsync();
    }

    public async Task<SubscriptionPlan> GetPlanByIdAsync(int id)
    {
        return await _context.SubscriptionPlans
            .FirstOrDefaultAsync(plan => plan.SubscriptionPlanID == id);
    }

    public async Task<SubscriptionPlan> GetByNameAsync(string planName)
    {
        return await _context.SubscriptionPlans
        .FirstOrDefaultAsync(s => s.PlanName.ToLower() == planName.ToLower());
    }

    public async Task AddSubscriptionPlanAsync(SubscriptionPlan plan)
    {
        await _context.SubscriptionPlans.AddAsync(plan);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSubscriptionPlanAsync(SubscriptionPlan plan)
    {
        _context.SubscriptionPlans.Update(plan);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSubscriptionPlanAsync(int id)
    {
        var plan = await GetPlanByIdAsync(id);
        if (plan != null)
        {
            _context.SubscriptionPlans.Remove(plan);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<CompanySubscriptionHistory>> GetSubscriptionHistoryAsync(int companyId)
    {
        return await _context.CompanySubscriptionHistories
            .Where(sh => sh.CorporateCompanyId == companyId)
            .ToListAsync();
    }
}
