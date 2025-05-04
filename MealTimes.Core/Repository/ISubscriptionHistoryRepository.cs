using MealTimes.Core.Models;

namespace MealTimes.Core.Repository
{
    public interface ISubscriptionHistoryRepository
    {
        Task<List<CompanySubscriptionHistory>> GetByCompanyIdAsync(int companyId);
        Task AddAsync(CompanySubscriptionHistory subscriptionHistory);
        Task<SubscriptionPlan?> GetByIdAsync(int planId);
    }
}
