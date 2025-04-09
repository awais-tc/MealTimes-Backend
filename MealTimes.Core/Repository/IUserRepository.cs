using MealTimes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.Repository
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string email);
        Task AddUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);

        Task AddAdminAsync(Admin admin);
        Task AddCompanyAsync(CorporateCompany company);
        Task AddEmployeeAsync(Employee employee);
        Task AddHomeChefAsync(HomeChef chef);

        Task<IEnumerable<User>> GetAllUsersAsync(); 
        Task<User> GetUserByIdAsync(int id);
        Task DeleteUserAsync(int id);

        Task SaveChangesAsync();
    }


}
