using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MealTimes.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<Admin, AdminDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<HomeChef, HomeChefDto>();
            CreateMap<CorporateCompany, CorporateCompanyDto>();

            // Register DTOs
            CreateMap<AdminRegisterDto, Admin>();
            CreateMap<EmployeeRegisterDto, Employee>();
            CreateMap<HomeChefRegisterDto, HomeChef>();
            CreateMap<CorporateCompanyRegisterDto, CorporateCompany>();
        }
    }
}
