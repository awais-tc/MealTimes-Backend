using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;

namespace MealTimes.Service
{
    public class CorporateCompanyService : ICorporateCompanyService
    {
        private readonly ICorporateCompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CorporateCompanyService(ICorporateCompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<GenericResponse<List<CorporateCompanyDto>>> GetAllCompaniesAsync()
        {
            var companies = await _companyRepository.GetAllCompaniesAsync();

            var companyDtos = companies.Select(company =>
            {
                var dto = _mapper.Map<CorporateCompanyDto>(company);
                dto.ActivePlanName = company.ActiveSubscriptionPlan?.PlanName;
                return dto;
            }).ToList();

            return new GenericResponse<List<CorporateCompanyDto>>
            {
                Data = companyDtos,
                IsSuccess = true,
                Message = "Companies fetched successfully"
            };
        }
    }
}
