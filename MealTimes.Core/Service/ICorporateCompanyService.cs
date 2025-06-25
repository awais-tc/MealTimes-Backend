using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;

namespace MealTimes.Core.Service
{
    public interface ICorporateCompanyService
    {
        Task<GenericResponse<List<CorporateCompanyDto>>> GetAllCompaniesAsync();
        Task<GenericResponse<CorporateCompanyDto>> GetByIdAsync(int id);
        Task<GenericResponse<CorporateCompanyDto>> UpdateAsync(UpdateCorporateCompanyDto dto);
        Task<GenericResponse<bool>> DeleteAsync(int id);
    }
}
