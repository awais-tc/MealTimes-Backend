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

            //Meal
            CreateMap<MealCreateDto, Meal>()
            .ForMember(dest => dest.ChefID, opt => opt.MapFrom(src => src.ChefID));

            CreateMap<MealUpdateDto, Meal>()
                .ForMember(dest => dest.MealID, opt => opt.MapFrom(src => src.MealID));

            CreateMap<Meal, MealDto>()
                .ForMember(dest => dest.ChefName, opt => opt.MapFrom(src => src.Chef.FullName));  // Assuming Chef is a related entity

            CreateMap<Meal, MealCreateDto>().ReverseMap();
            CreateMap<Meal, MealUpdateDto>().ReverseMap();

            //Subscription Plan
            CreateMap<SubscriptionPlan, SubscriptionPlanDto>().ReverseMap();

            // Create DTO → Domain
            CreateMap<SubscriptionPlanCreateDto, SubscriptionPlan>();

            // Update DTO → Domain
            CreateMap<SubscriptionPlanUpdateDto, SubscriptionPlan>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<SubscriptionHistoryDto, CompanySubscriptionHistory>();
        }
    }
}
