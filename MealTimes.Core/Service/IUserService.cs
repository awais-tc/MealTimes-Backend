using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.Service
{
    public interface IUserService
    {
        Task<GenericResponse<UserDto>> RegisterAdminAsync(AdminRegisterDto dto);
        Task<GenericResponse<UserDto>> RegisterCompanyAsync(CorporateCompanyRegisterDto dto);
        Task<GenericResponse<UserDto>> RegisterEmployeeAsync(EmployeeRegisterDto dto);
        Task<GenericResponse<UserDto>> RegisterHomeChefAsync(HomeChefRegisterDto dto);
        Task<GenericResponse<AuthResponseDto>> LoginAsync(LoginDto dto);
        Task<GenericResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<GenericResponse<UserDto>> GetUserByIdAsync(int id);
        Task<GenericResponse<bool>> DeleteUserAsync(int id);
    }
}
