using MealTimes.Core.Models;

namespace MealTimes.Core.Repository
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeeWithCompanyAsync(int employeeId);
    }
}
