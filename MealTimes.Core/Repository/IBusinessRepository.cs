using MealTimes.Core.Models;

namespace MealTimes.Core.Repository
{
    public interface IBusinessRepository
    {
        // Commission Repository
        Task<Commission> CreateCommissionAsync(Commission commission);
        Task<Commission?> GetCommissionByOrderIdAsync(int orderId);
        Task<List<Commission>> GetCommissionsByChefAsync(int chefId, DateTime? startDate = null, DateTime? endDate = null);
        Task<List<Commission>> GetAllCommissionsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Commission> UpdateCommissionAsync(Commission commission);

        // Chef Payout Repository
        Task<ChefPayout> CreateChefPayoutAsync(ChefPayout payout);
        Task<ChefPayout?> GetChefPayoutByIdAsync(int payoutId);
        Task<List<ChefPayout>> GetChefPayoutsAsync(int? chefId = null, string? status = null);
        Task<ChefPayout> UpdateChefPayoutAsync(ChefPayout payout);
        Task<List<ChefPayout>> GetPendingPayoutsAsync();
        Task<bool> HasPendingPayoutForPeriodAsync(int chefId, DateTime startDate, DateTime endDate);

        // Business Metrics Repository
        Task<BusinessMetrics> CreateBusinessMetricsAsync(BusinessMetrics metrics);
        Task<BusinessMetrics?> GetBusinessMetricsByDateAsync(DateTime date);
        Task<List<BusinessMetrics>> GetBusinessMetricsRangeAsync(DateTime startDate, DateTime endDate);
        Task<BusinessMetrics> UpdateBusinessMetricsAsync(BusinessMetrics metrics);

        // Analytics Queries
        Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetTotalCommissionRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetTotalSubscriptionRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetTotalChefPayoutsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<int> GetTotalOrdersAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetAverageOrderValueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<int> GetActiveSubscriptionsCountAsync();
        Task<int> GetActiveChefsCountAsync();
        Task<int> GetActiveEmployeesCountAsync();

        Task SaveChangesAsync();
    }
}