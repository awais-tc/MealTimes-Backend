using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;
using Microsoft.Extensions.Configuration;
using Stripe;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepo;
    private readonly ICorporateCompanyRepository _companyRepo;
    private readonly ISubscriptionPlanRepository _planRepo;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public PaymentService(
        IPaymentRepository paymentRepo,
        ICorporateCompanyRepository companyRepo,
        ISubscriptionPlanRepository planRepo,
        IMapper mapper,
        IConfiguration config)
    {
        _paymentRepo = paymentRepo;
        _companyRepo = companyRepo;
        _planRepo = planRepo;
        _mapper = mapper;
        _config = config;
    }

    public async Task<GenericResponse<PaymentResponseDto>> ProcessSubscriptionPaymentAsync(PaymentRequestDto dto)
    {
        var company = await _companyRepo.GetByIdAsync(dto.CompanyId);
        if (company == null) return GenericResponse<PaymentResponseDto>.Fail("Company not found.");

        var plan = await _planRepo.GetPlanByIdAsync(dto.SubscriptionPlanId);
        if (plan == null) return GenericResponse<PaymentResponseDto>.Fail("Plan not found.");

        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

        var options = new ChargeCreateOptions
        {
            Amount = (long)(plan.Price * 100),
            Currency = "usd",
            Description = $"Subscription for {company.CompanyName}",
            Source = dto.StripeToken
        };

        var service = new ChargeService();
        var charge = await service.CreateAsync(options);

        if (charge.Status != "succeeded")
            return GenericResponse<PaymentResponseDto>.Fail("Payment failed.");

        var payment = new Payment
        {
            CorporateCompanyID = company.CompanyID,
            SubscriptionPlanID = plan.SubscriptionPlanID,
            PaymentAmount = plan.Price,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = "Stripe",
            PaymentStatus = charge.Status,
            StripeSessionId = charge.Id
        };

        await _paymentRepo.AddAsync(payment);

        company.ActiveSubscriptionPlanID = plan.SubscriptionPlanID;
        company.PlanStartDate = DateTime.UtcNow;
        company.PlanEndDate = DateTime.UtcNow.AddDays(plan.DurationInDays);
        await _companyRepo.UpdateAsync(company);

        var responseDto = _mapper.Map<PaymentResponseDto>(payment);
        return GenericResponse<PaymentResponseDto>.Success(responseDto, "Payment successful");
    }

    public async Task<GenericResponse<List<PaymentResponseDto>>> GetAllPaymentsAsync()
    {
        var payments = await _paymentRepo.GetAllPaymentsAsync();
        var dtoList = _mapper.Map<List<PaymentResponseDto>>(payments);
        return GenericResponse<List<PaymentResponseDto>>.Success(dtoList);
    }
}
