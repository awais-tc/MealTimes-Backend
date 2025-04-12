using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.DTOs
{
    public class SubscriptionPlanDto
    {
        public int SubscriptionPlanID { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public int MealLimitPerDay { get; set; }
        public int DurationInDays { get; set; }
        public bool IsCustomizable { get; set; }
        public int MaxEmployees { get; set; }
    }


    public class SubscriptionPlanCreateDto
    {
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public int MealLimitPerDay { get; set; }
        public int DurationInDays { get; set; }
        public bool IsCustomizable { get; set; }
        public int MaxEmployees { get; set; }
    }

    public class SubscriptionPlanUpdateDto
    {
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public int MealLimitPerDay { get; set; }
        public int DurationInDays { get; set; }
        public bool IsCustomizable { get; set; }
        public int MaxEmployees { get; set; }
    }

    public class SubscriptionHistoryDto
    {
        public int SubscriptionHistoryID { get; set; }
        public int CorporateCompanyID { get; set; }
        public int SubscriptionPlanID { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }

}
