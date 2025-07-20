using System.ComponentModel.DataAnnotations;

namespace MealTimes.Core.DTOs
{
    public class CommissionDto
    {
        public int CommissionID { get; set; }
        public int OrderID { get; set; }
        public int ChefID { get; set; }
        public string ChefName { get; set; } = string.Empty;
        public decimal OrderAmount { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal ChefPayableAmount { get; set; }
        public decimal PlatformEarning { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class BusinessMetricsDto
    {
        public DateTime Date { get; set; }
        public decimal SubscriptionRevenue { get; set; }
        public decimal CommissionRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal ChefPayouts { get; set; }
        public decimal OperationalCosts { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetProfit { get; set; }
        public decimal ProfitMargin { get; set; }
        public int TotalOrders { get; set; }
        public int ActiveSubscriptions { get; set; }
        public int ActiveChefs { get; set; }
        public int ActiveEmployees { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    public class ChefPayoutDto
    {
        public int PayoutID { get; set; }
        public int ChefID { get; set; }
        public string ChefName { get; set; } = string.Empty;
        public decimal TotalEarnings { get; set; }
        public decimal CommissionDeducted { get; set; }
        public decimal PayableAmount { get; set; }
        public string PayoutPeriod { get; set; } = string.Empty;
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public int TotalOrders { get; set; }
    }

    public class CreateChefPayoutDto
    {
        [Required]
        public int ChefID { get; set; }
        
        [Required]
        public string PayoutPeriod { get; set; } = string.Empty; // Weekly, Monthly
        
        [Required]
        public DateTime PeriodStart { get; set; }
        
        [Required]
        public DateTime PeriodEnd { get; set; }
    }

    public class UpdatePayoutStatusDto
    {
        [Required]
        public int PayoutID { get; set; }
        
        [Required]
        public string Status { get; set; } = string.Empty;
        
        public string? PaymentMethod { get; set; }
        public string? PaymentReference { get; set; }
        public string? Notes { get; set; }
    }

    public class BusinessAnalyticsDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal TotalCommissions { get; set; }
        public decimal TotalChefPayouts { get; set; }
        public decimal ProfitMargin { get; set; }
        public int TotalOrders { get; set; }
        public int TotalActiveSubscriptions { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal MonthlyGrowthRate { get; set; }
        
        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
        public List<TopChefDto> TopChefs { get; set; } = new();
        public List<TopCompanyDto> TopCompanies { get; set; } = new();
    }

    public class MonthlyRevenueDto
    {
        public string Month { get; set; } = string.Empty;
        public decimal SubscriptionRevenue { get; set; }
        public decimal CommissionRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal NetProfit { get; set; }
    }

    public class TopChefDto
    {
        public int ChefID { get; set; }
        public string ChefName { get; set; } = string.Empty;
        public decimal TotalEarnings { get; set; }
        public decimal CommissionGenerated { get; set; }
        public int TotalOrders { get; set; }
        public double Rating { get; set; }
    }

    public class TopCompanyDto
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public decimal TotalSubscriptionPaid { get; set; }
        public decimal TotalOrderValue { get; set; }
        public int TotalOrders { get; set; }
        public int EmployeeCount { get; set; }
    }

    public class ProfitLossReportDto
    {
        public DateTime ReportDate { get; set; }
        public string Period { get; set; } = string.Empty; // Daily, Weekly, Monthly, Yearly
        
        // Revenue Breakdown
        public decimal SubscriptionRevenue { get; set; }
        public decimal CommissionRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
        
        // Expense Breakdown
        public decimal ChefPayouts { get; set; }
        public decimal OperationalCosts { get; set; }
        public decimal MarketingCosts { get; set; }
        public decimal TechnologyCosts { get; set; }
        public decimal TotalExpenses { get; set; }
        
        // Profit Analysis
        public decimal GrossProfit { get; set; }
        public decimal NetProfit { get; set; }
        public decimal ProfitMargin { get; set; }
        
        // Key Metrics
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal CustomerAcquisitionCost { get; set; }
        public decimal CustomerLifetimeValue { get; set; }
    }
}