using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealTimes.Core.Models
{
    public class BusinessMetrics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MetricsID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        // Revenue Streams
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubscriptionRevenue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CommissionRevenue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalRevenue { get; set; }

        // Expenses
        [Column(TypeName = "decimal(18,2)")]
        public decimal ChefPayouts { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OperationalCosts { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalExpenses { get; set; }

        // Profit/Loss
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetProfit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProfitMargin { get; set; }

        // Order Statistics
        public int TotalOrders { get; set; }
        public int ActiveSubscriptions { get; set; }
        public int ActiveChefs { get; set; }
        public int ActiveEmployees { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AverageOrderValue { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}