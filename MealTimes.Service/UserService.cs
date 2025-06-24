using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;

namespace MealTimes.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICorporateCompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHomeChefRepository _homeChefRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;

        public UserService(IUserRepository userRepository,
            IMapper mapper, 
            IJwtTokenGenerator jwtTokenGenerator, 
            ICorporateCompanyRepository companyRepository, 
            IEmployeeRepository employeeRepository, 
            IHomeChefRepository homeChefRepository,
            IDeliveryPersonRepository deliveryPersonRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _homeChefRepository = homeChefRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
        }

        public async Task<GenericResponse<UserDto>> RegisterAdminAsync(AdminRegisterDto dto)
        {
            if (await _userRepository.UserExistsAsync(dto.Email))
                return GenericResponse<UserDto>.Fail("Email already in use.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = "Admin",
                Admin = new Admin
                {
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber
                }
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return GenericResponse<UserDto>.Success(_mapper.Map<UserDto>(user), "Admin registered successfully.");
        }

        public async Task<GenericResponse<UserDto>> RegisterCompanyAsync(CorporateCompanyRegisterDto dto)
        {
            if (await _userRepository.UserExistsAsync(dto.Email))
                return GenericResponse<UserDto>.Fail("Email already in use.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = "Company",
                CorporateCompany = new CorporateCompany
                {
                    CompanyName = dto.CompanyName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    AdminID = dto.AdminID,
                    ActiveSubscriptionPlanID = dto.SubscriptionPlanID
                }
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return GenericResponse<UserDto>.Success(_mapper.Map<UserDto>(user), "Company registered successfully.");
        }

        public async Task<GenericResponse<UserDto>> RegisterEmployeeAsync(EmployeeRegisterDto dto)
        {
            if (await _userRepository.UserExistsAsync(dto.Email))
                return GenericResponse<UserDto>.Fail("Email already in use.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = "Employee",
                Employee = new Employee
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    DietaryPreferences = dto.DietaryPreferences,
                    CompanyID = dto.CompanyID
                }
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return GenericResponse<UserDto>.Success(_mapper.Map<UserDto>(user), "Employee registered successfully.");
        }

        public async Task<GenericResponse<UserDto>> RegisterHomeChefAsync(HomeChefRegisterDto dto)
        {
            if (await _userRepository.UserExistsAsync(dto.Email))
                return GenericResponse<UserDto>.Fail("Email already in use.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = "Chef",
                HomeChef = new HomeChef
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    Rating = 0
                }
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return GenericResponse<UserDto>.Success(_mapper.Map<UserDto>(user), "Home Chef registered successfully.");
        }

        public async Task<GenericResponse<UserDto>> RegisterDeliveryPersonAsync(DeliveryPersonRegisterDto dto)
        {
            if (await _userRepository.UserExistsAsync(dto.Email))
                return GenericResponse<UserDto>.Fail("Email already in use.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = "DeliveryPerson",
                DeliveryPerson = new DeliveryPerson
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    VehicleInfo = dto.VehicleInfo
                }
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return GenericResponse<UserDto>.Success(_mapper.Map<UserDto>(user), "Delivery person registered successfully.");
        }

        public async Task<GenericResponse<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return GenericResponse<AuthResponseDto>.Fail("Invalid credentials");

            var token = _jwtTokenGenerator.GenerateToken(user);
            return GenericResponse<AuthResponseDto>.Success(new AuthResponseDto
            {
                Token = token,
                UserDto = _mapper.Map<UserDto>(user)
            }, "Login successful.");
        }

        public async Task<GenericResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return GenericResponse<IEnumerable<UserDto>>.Success(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        public async Task<GenericResponse<UserDto>> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user == null
                ? GenericResponse<UserDto>.Fail("User not found")
                : GenericResponse<UserDto>.Success(_mapper.Map<UserDto>(user));
        }

        public async Task<GenericResponse<bool>> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return GenericResponse<bool>.Fail("User not found.");

            // Delete role-specific entity first
            switch (user.Role)
            {
                case "Company":
                    if (user.CorporateCompany != null)
                        await _companyRepository.DeleteAsync(user.CorporateCompany.CompanyID);
                    break;

                case "Employee":
                    if (user.Employee != null)
                        await _employeeRepository.DeleteAsync(user.Employee.EmployeeID);
                    break;

                case "Chef":
                    if (user.HomeChef != null)
                        await _homeChefRepository.DeleteAsync(user.HomeChef.ChefID);
                    break;
            }

            // Delete user after role entity is removed
            await _userRepository.DeleteUserAsync(id);
            await _userRepository.SaveChangesAsync();

            return GenericResponse<bool>.Success(true, "User deleted successfully.");
        }
    }
}
