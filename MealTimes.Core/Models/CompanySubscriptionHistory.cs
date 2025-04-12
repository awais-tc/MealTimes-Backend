using MealTimes.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace MealTimes.Core.Models;
public class CompanySubscriptionHistory
{
    [Key]
    public int Id { get; set; }

    public int CorporateCompanyId { get; set; }
    public CorporateCompany CorporateCompany { get; set; }

    public int SubscriptionPlanID { get; set; }
    public SubscriptionPlan SubscriptionPlan { get; set; }

    public DateTime SubscribedOn { get; set; }
    public DateTime? EndedOn { get; set; }
}
