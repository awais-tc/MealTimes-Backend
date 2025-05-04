using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MealTimes.Core.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }

        public int? OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order? Order { get; set; }

        public int? SubscriptionPlanID { get; set; }
        [ForeignKey("SubscriptionPlanID")]
        public virtual SubscriptionPlan? SubscriptionPlan { get; set; }

        public int? CorporateCompanyID { get; set; }
        [ForeignKey("CorporateCompanyID")]
        public virtual CorporateCompany? CorporateCompany { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentAmount { get; set; }

        public DateTime PaymentDate { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // "Stripe", etc.

        [Required]
        public string PaymentStatus { get; set; } // "Pending", "Succeeded", etc.

        public string? StripeSessionId { get; set; } // Optional Stripe reference
    }
}
