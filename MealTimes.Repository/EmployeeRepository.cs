using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace MealTimes.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetEmployeeWithCompanyAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.CorporateCompany)
                .ThenInclude(c => c.ActiveSubscriptionPlan)
                .FirstOrDefaultAsync(e => e.EmployeeID == employeeId);

        }
    }
}
