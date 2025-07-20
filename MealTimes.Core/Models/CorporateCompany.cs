using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MealTimes.Core.Models
{
    public class CorporateCompany
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyID { get; set; }

        [MaxLength(100)]
        public required string CompanyName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public required string Address { get; set; }

        public int? AdminID { get; set; }

        public int? ActiveSubscriptionPlanID { get; set; }

        [ForeignKey("ActiveSubscriptionPlanID")]
        public SubscriptionPlan? ActiveSubscriptionPlan { get; set; }

        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }

        public int? LocationID { get; set; }
        [ForeignKey("LocationID")]
        public virtual Location? Location { get; set; }

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}