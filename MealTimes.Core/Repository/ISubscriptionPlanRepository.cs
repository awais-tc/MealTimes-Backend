using MealTimes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.Repository
{
    public interface ISubscriptionPlanRepository
    {
        Task<List<SubscriptionPlan>> GetAllPlansAsync();
        Task<SubscriptionPlan> GetPlanByIdAsync(int id);
        Task<SubscriptionPlan> GetByNameAsync(string planName);
        Task AddSubscriptionPlanAsync(SubscriptionPlan plan);
        Task UpdateSubscriptionPlanAsync(SubscriptionPlan plan);
        Task DeleteSubscriptionPlanAsync(int id);
        Task<List<CompanySubscriptionHistory>> GetSubscriptionHistoryAsync(int companyId);
    }

}
