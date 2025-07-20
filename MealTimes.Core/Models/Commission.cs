using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealTimes.Core.Models
{
    public class Commission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommissionID { get; set; }

        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        public int ChefID { get; set; }
        [ForeignKey("ChefID")]
        public virtual HomeChef Chef { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal CommissionRate { get; set; } // e.g., 20.00 for 20%

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CommissionAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ChefPayableAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PlatformEarning { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Paid, Failed

        public DateTime? PaidAt { get; set; }
        public string? PaymentReference { get; set; }
    }
}