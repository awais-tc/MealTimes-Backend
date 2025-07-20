using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealTimes.Core.Models
{
    public class ChefPayout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayoutID { get; set; }

        public int ChefID { get; set; }
        [ForeignKey("ChefID")]
        public virtual HomeChef Chef { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalEarnings { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CommissionDeducted { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PayableAmount { get; set; }

        [Required]
        public string PayoutPeriod { get; set; } // Weekly, Monthly

        [Required]
        public DateTime PeriodStart { get; set; }

        [Required]
        public DateTime PeriodEnd { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Processing, Completed, Failed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }

        public string? PaymentMethod { get; set; }
        public string? PaymentReference { get; set; }
        public string? Notes { get; set; }

        // Navigation property for related commissions
        public virtual ICollection<Commission> Commissions { get; set; } = new List<Commission>();
    }
}