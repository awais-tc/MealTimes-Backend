﻿

namespace MealTimes.Core.DTOs
{
    public class CreatePaymentDto
    {
        public int? OrderID { get; set; }
        public int? CompanySubscriptionHistoryID { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentMethod { get; set; } // "card" for now
        public string StripeToken { get; set; }   // Required for Stripe
    }


    public class PaymentDto
    {
        public int PaymentID { get; set; }
        public int? OrderID { get; set; }
        public int? CompanySubscriptionHistoryID { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class PaymentRequestDto
    {
        public int CompanyId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string StripeToken { get; set; } = default!;
    }

    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string? StripeSessionId { get; set; }

        public int? SubscriptionPlanId { get; set; }
        public string? PlanName { get; set; }

        public int? CorporateCompanyId { get; set; }
        public string? CompanyName { get; set; }
    }
}
