using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;

namespace MealTimes.Core.Service
{
    public interface IEmployeeService
    {
        Task<GenericResponse<List<EmployeeDto>>> GetEmployeesByCompanyIdAsync(int companyId);
        Task<GenericResponse<EmployeeDto>> GetByIdAsync(int employeeId);
        Task<GenericResponse<EmployeeDto>> UpdateAsync(UpdateEmployeeDto dto);
    }
}
