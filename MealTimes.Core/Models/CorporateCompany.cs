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
        public required string  CompanyName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public required string Address { get; set; }
        public int AdminID { get; set; }
        public int SubscriptionPlanID { get; set; }
        [ForeignKey("SubscriptionPlanID")]
        public SubscriptionPlan SubscriptionPlan { get; set; }

        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
