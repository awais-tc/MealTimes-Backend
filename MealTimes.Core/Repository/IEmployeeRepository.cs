using MealTimes.Core.Models;

namespace MealTimes.Core.Repository
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeeWithCompanyAsync(int employeeId);
        Task<Employee?> GetByUserIdAsync(int userId);
        Task<Employee?> GetByIdAsync(int employeeId);
        Task<List<Employee>> GetEmployeesByCompanyIdAsync(int companyId);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
    }
}
