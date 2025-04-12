using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;

public class SubscriptionPlanService : ISubscriptionPlanService
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly ICorporateCompanyRepository _corporateCompanyRepository;
    private readonly IMapper _mapper;
    private readonly ISubscriptionHistoryRepository _subscriptionHistoryRepository;

    public SubscriptionPlanService(
        ISubscriptionPlanRepository subscriptionPlanRepository,
        ICorporateCompanyRepository corporateCompanyRepository,
        IMapper mapper,
        ISubscriptionHistoryRepository subscriptionHistoryRepository)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _corporateCompanyRepository = corporateCompanyRepository;
        _mapper = mapper;
        _subscriptionHistoryRepository = subscriptionHistoryRepository;
    }

    public async Task<GenericResponse<List<SubscriptionPlanDto>>> GetAllPlansAsync()
    {
        var plans = await _subscriptionPlanRepository.GetAllPlansAsync();
        if (plans == null || !plans.Any())
        {
            return new GenericResponse<List<SubscriptionPlanDto>>
            {
                IsSuccess = false,
                Message = "No subscription plans found."
            };
        }

        var planDtos = _mapper.Map<List<SubscriptionPlanDto>>(plans);
        return new GenericResponse<List<SubscriptionPlanDto>>
        {
            IsSuccess = true,
            Data = planDtos,
            Message = "Subscription plans fetched successfully."
        };
    }

    public async Task<GenericResponse<SubscriptionPlanDto>> GetPlanByIdAsync(int subscriptionPlanID)
    {
        var plan = await _subscriptionPlanRepository.GetPlanByIdAsync(subscriptionPlanID);
        if (plan == null)
        {
            return new GenericResponse<SubscriptionPlanDto>
            {
                IsSuccess = false,
                Message = "Subscription plan not found."
            };
        }

        var planDto = _mapper.Map<SubscriptionPlanDto>(plan);
        return new GenericResponse<SubscriptionPlanDto>
        {
            IsSuccess = true,
            Data = planDto,
            Message = "Subscription plan fetched successfully."
        };
    }

    public async Task<GenericResponse<SubscriptionPlanDto>> CreateSubscriptionPlanAsync(SubscriptionPlanCreateDto subscriptionPlanCreateDto)
    {
        var existingPlan = await _subscriptionPlanRepository.GetByNameAsync(subscriptionPlanCreateDto.PlanName);
        if (existingPlan != null)
        {
            return new GenericResponse<SubscriptionPlanDto>
            {
                IsSuccess = false,
                Message = "A plan with this name already exists."
            };
        }

        var newPlan = _mapper.Map<SubscriptionPlan>(subscriptionPlanCreateDto);
        await _subscriptionPlanRepository.AddSubscriptionPlanAsync(newPlan);

        var planDto = _mapper.Map<SubscriptionPlanDto>(newPlan);
        return new GenericResponse<SubscriptionPlanDto>
        {
            IsSuccess = true,
            Data = planDto,
            Message = "Subscription plan created successfully."
        };
    }

    public async Task<GenericResponse<SubscriptionPlanDto>> UpdateSubscriptionPlanAsync(int subscriptionPlanID, SubscriptionPlanUpdateDto subscriptionPlanUpdateDto)
    {
        var plan = await _subscriptionPlanRepository.GetPlanByIdAsync(subscriptionPlanID);
        if (plan == null)
        {
            return new GenericResponse<SubscriptionPlanDto>
            {
                IsSuccess = false,
                Message = "Subscription plan not found."
            };
        }

        _mapper.Map(subscriptionPlanUpdateDto, plan);
        await _subscriptionPlanRepository.UpdateSubscriptionPlanAsync(plan);

        var updatedPlanDto = _mapper.Map<SubscriptionPlanDto>(plan);
        return new GenericResponse<SubscriptionPlanDto>
        {
            IsSuccess = true,
            Data = updatedPlanDto,
            Message = "Subscription plan updated successfully."
        };
    }

    public async Task<GenericResponse<SubscriptionPlanDto>> DeleteSubscriptionPlanAsync(int subscriptionPlanID)
    {
        var plan = await _subscriptionPlanRepository.GetPlanByIdAsync(subscriptionPlanID);
        if (plan == null)
        {
            return new GenericResponse<SubscriptionPlanDto>
            {
                IsSuccess = false,
                Message = "Subscription plan not found."
            };
        }

        await _subscriptionPlanRepository.DeleteSubscriptionPlanAsync(subscriptionPlanID);
        return new GenericResponse<SubscriptionPlanDto>
        {
            IsSuccess = true,
            Message = "Subscription plan deleted successfully."
        };
    }

    public async Task<GenericResponse<List<SubscriptionHistoryDto>>> GetSubscriptionHistoryAsync(int companyID)
    {
        var history = await _subscriptionHistoryRepository.GetByCompanyIdAsync(companyID);
        if (history == null || !history.Any())
        {
            return new GenericResponse<List<SubscriptionHistoryDto>>
            {
                IsSuccess = false,
                Message = "No subscription history found."
            };
        }

        var historyDtos = _mapper.Map<List<SubscriptionHistoryDto>>(history);
        return new GenericResponse<List<SubscriptionHistoryDto>>
        {
            IsSuccess = true,
            Data = historyDtos,
            Message = "Subscription history fetched successfully."
        };
    }

    public async Task<GenericResponse<SubscriptionPlanDto>> SubscribeToPlanAsync(int companyID, int subscriptionPlanID)
    {
        var company = await _corporateCompanyRepository.GetByIdAsync(companyID);
        if (company == null)
        {
            return new GenericResponse<SubscriptionPlanDto>
            {
                IsSuccess = false,
                Message = "Company not found."
            };
        }

        var plan = await _subscriptionPlanRepository.GetPlanByIdAsync(subscriptionPlanID);
        if (plan == null)
        {
            return new GenericResponse<SubscriptionPlanDto>
            {
                IsSuccess = false,
                Message = "Subscription plan not found."
            };
        }

        // Check if company has available slots
        if (company.Employees.Count >= plan.MaxEmployees)
        {
            return new GenericResponse<SubscriptionPlanDto>
            {
                IsSuccess = false,
                Message = "Company has reached the maximum number of employees for this plan."
            };
        }

        // Proceed to subscribe the company to the plan
        company.ActiveSubscriptionPlanID = subscriptionPlanID;
        await _corporateCompanyRepository.UpdateAsync(company);

        // Log subscription history
        var subscriptionHistory = new CompanySubscriptionHistory
        {
            CorporateCompanyId = companyID,
            SubscriptionPlanID = subscriptionPlanID,
            SubscribedOn = DateTime.UtcNow
        };

        await _subscriptionHistoryRepository.AddAsync(subscriptionHistory);

        var planDto = _mapper.Map<SubscriptionPlanDto>(plan);
        return new GenericResponse<SubscriptionPlanDto>
        {
            IsSuccess = true,
            Data = planDto,
            Message = "Company subscribed to the plan successfully."
        };
    }
}
