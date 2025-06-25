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

        public async Task<Employee?> GetByUserIdAsync(int userId)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.UserID == userId);
        }

        public async Task<Employee?> GetByIdAsync(int employeeId)
        {
            return await _context.Employees.FindAsync(employeeId);
        }

        public async Task<List<Employee>> GetEmployeesByCompanyIdAsync(int companyId)
        {
            return await _context.Employees
                .Where(e => e.CompanyID == companyId)
                .Include(e => e.CorporateCompany)
                .ToListAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await GetByIdAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}
