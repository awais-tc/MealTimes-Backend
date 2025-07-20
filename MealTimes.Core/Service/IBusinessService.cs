using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;

namespace MealTimes.Core.Service
{
    public interface IBusinessService
    {
        // Commission Management
        Task<GenericResponse<CommissionDto>> CalculateCommissionAsync(int orderId);
        Task<GenericResponse<List<CommissionDto>>> GetCommissionsByChefAsync(int chefId, DateTime? startDate = null, DateTime? endDate = null);
        Task<GenericResponse<List<CommissionDto>>> GetAllCommissionsAsync(DateTime? startDate = null, DateTime? endDate = null);

        // Chef Payout Management
        Task<GenericResponse<ChefPayoutDto>> CreateChefPayoutAsync(CreateChefPayoutDto dto);
        Task<GenericResponse<List<ChefPayoutDto>>> GetChefPayoutsAsync(int? chefId = null, string? status = null);
        Task<GenericResponse<ChefPayoutDto>> UpdatePayoutStatusAsync(UpdatePayoutStatusDto dto);
        Task<GenericResponse<List<ChefPayoutDto>>> GetPendingPayoutsAsync();

        // Business Analytics
        Task<GenericResponse<BusinessAnalyticsDto>> GetBusinessAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<GenericResponse<ProfitLossReportDto>> GenerateProfitLossReportAsync(DateTime startDate, DateTime endDate, string period = "Monthly");
        Task<GenericResponse<BusinessMetricsDto>> GetDailyMetricsAsync(DateTime date);
        Task<GenericResponse<List<BusinessMetricsDto>>> GetMetricsRangeAsync(DateTime startDate, DateTime endDate);

        // Financial Operations
        Task<GenericResponse<bool>> ProcessDailyMetricsAsync(DateTime date);
        Task<GenericResponse<bool>> ProcessWeeklyPayoutsAsync();
        Task<GenericResponse<bool>> ProcessMonthlyPayoutsAsync();
    }
}