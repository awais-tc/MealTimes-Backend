using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;

namespace MealTimes.Core.Service
{
    public interface ICorporateCompanyService
    {
        Task<GenericResponse<List<CorporateCompanyDto>>> GetAllCompaniesAsync();
    }
}
