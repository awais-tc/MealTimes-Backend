

using System.ComponentModel.DataAnnotations;

namespace MealTimes.Core.DTOs
{
    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Admin", "CorporateCompany", "Employee", "HomeChef"
    }

    public class UserDto
    {
        public int UserID { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        // Optional role-specific details
        public AdminDto? Admin { get; set; }
        public CorporateCompanyDto? CorporateCompany { get; set; }
        public EmployeeDto? Employee { get; set; }
        public HomeChefDto? HomeChef { get; set; }
    }

    public class AdminDto
    {
        public int AdminID { get; set; }

        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public int UserID { get; set; }
    }

    public class CorporateCompanyDto
    {
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string Address { get; set; }

        public int SubscriptionPlanID { get; set; }

        public int AdminID { get; set; }

        public int UserID { get; set; }
    }

    public class EmployeeDto
    {
        public int EmployeeID { get; set; }

        public int CompanyID { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string? DietaryPreferences { get; set; }

        public int UserID { get; set; }
    }


    public class HomeChefDto
    {
        public int ChefID { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int UserID { get; set; }
    }


    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto UserDto { get; set; }
    }

    public class AdminRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class CorporateCompanyRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Address { get; set; }
        public int AdminID { get; set; } // Must be existing admin
        public int SubscriptionPlanID { get; set; }
    }


    public class EmployeeCreateDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string DietaryPreferences { get; set; }
        public int CompanyID { get; set; }
    }

    public class EmployeeRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string DietaryPreferences { get; set; }
        public int CompanyID { get; set; } // Must be existing company
    }


    public class HomeChefCreateDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class HomeChefRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }



}
