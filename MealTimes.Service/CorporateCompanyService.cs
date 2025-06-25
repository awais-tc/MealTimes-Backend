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

        public async Task<GenericResponse<CorporateCompanyDto>> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return new GenericResponse<CorporateCompanyDto>
                {
                    IsSuccess = false,
                    Message = "Company not found"
                };
            }

            var dto = _mapper.Map<CorporateCompanyDto>(company);
            dto.ActivePlanName = company.ActiveSubscriptionPlan?.PlanName;

            return new GenericResponse<CorporateCompanyDto>
            {
                Data = dto,
                IsSuccess = true,
                Message = "Company fetched successfully"
            };
        }

        public async Task<GenericResponse<CorporateCompanyDto>> UpdateAsync(UpdateCorporateCompanyDto dto)
        {
            var existing = await _companyRepository.GetByIdAsync(dto.CompanyID);
            if (existing == null)
            {
                return new GenericResponse<CorporateCompanyDto>
                {
                    IsSuccess = false,
                    Message = "Company not found"
                };
            }

            _mapper.Map(dto, existing);
            await _companyRepository.UpdateAsync(existing);

            var resultDto = _mapper.Map<CorporateCompanyDto>(existing);
            return new GenericResponse<CorporateCompanyDto>
            {
                Data = resultDto,
                IsSuccess = true,
                Message = "Company updated successfully"
            };
        }

        public async Task<GenericResponse<bool>> DeleteAsync(int id)
        {
            var existing = await _companyRepository.GetByIdAsync(id);
            if (existing == null)
            {
                return new GenericResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Company not found",
                    Data = false
                };
            }

            await _companyRepository.DeleteAsync(id);
            return new GenericResponse<bool>
            {
                IsSuccess = true,
                Message = "Company deleted successfully",
                Data = true
            };
        }
    }
}
