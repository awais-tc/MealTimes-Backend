using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Repository;
using Microsoft.EntityFrameworkCore;

namespace TheMealTimes.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Admin)
                .Include(u => u.CorporateCompany)
                .Include(u => u.Employee)
                .Include(u => u.HomeChef)
                .Include(u => u.DeliveryPerson)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAdminAsync(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
        }

        public async Task AddCompanyAsync(CorporateCompany company)
        {
            await _context.CorporateCompanies.AddAsync(company);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        public async Task AddHomeChefAsync(HomeChef chef)
        {
            await _context.HomeChefs.AddAsync(chef);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Admin)
                .Include(u => u.HomeChef)
                .Include(u => u.CorporateCompany)
                .Include(u => u.Employee)
                .Include(u => u.DeliveryPerson)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Admin)
                .Include(u => u.HomeChef)
                .Include(u => u.CorporateCompany)
                .Include(u => u.Employee)
                .Include(u => u.DeliveryPerson)
                .FirstOrDefaultAsync(u => u.UserID == id);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
