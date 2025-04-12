using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.Service
{
    public interface ISubscriptionPlanService
    {
        Task<GenericResponse<List<SubscriptionPlanDto>>> GetAllPlansAsync();
        Task<GenericResponse<SubscriptionPlanDto>> GetPlanByIdAsync(int subscriptionPlanID);
        Task<GenericResponse<SubscriptionPlanDto>> CreateSubscriptionPlanAsync(SubscriptionPlanCreateDto subscriptionPlanCreateDto);
        Task<GenericResponse<SubscriptionPlanDto>> UpdateSubscriptionPlanAsync(int subscriptionPlanID, SubscriptionPlanUpdateDto subscriptionPlanUpdateDto);
        Task<GenericResponse<SubscriptionPlanDto>> DeleteSubscriptionPlanAsync(int subscriptionPlanID);
        Task<GenericResponse<List<SubscriptionHistoryDto>>> GetSubscriptionHistoryAsync(int companyID);
        Task<GenericResponse<SubscriptionPlanDto>> SubscribeToPlanAsync(int companyID, int subscriptionPlanID);
    }
}
