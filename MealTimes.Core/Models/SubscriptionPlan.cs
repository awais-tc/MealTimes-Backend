
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MealTimes.Core.Models
{
    public class SubscriptionPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubscriptionPlanID { get; set; }

        [Required]
        public string PlanName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int MealLimitPerDay { get; set; }

        [Required]
        public int DurationInDays { get; set; }

        public bool IsCustomizable { get; set; } = false;

        [Required]
        public int MaxEmployees { get; set; }

        public ICollection<CorporateCompany> CompaniesUsingThisPlan { get; set; }
    }
}
