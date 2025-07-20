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
            CreateMap<CorporateCompany, CorporateCompanyDto>()
             .ForMember(dest => dest.ActivePlanName, opt => opt.Ignore());


            // Register DTOs
            CreateMap<AdminRegisterDto, Admin>();
            CreateMap<EmployeeRegisterDto, Employee>();
            CreateMap<HomeChefRegisterDto, HomeChef>();
            CreateMap<CorporateCompanyRegisterDto, CorporateCompany>();
            CreateMap<UpdateCorporateCompanyDto, CorporateCompany>();

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

            //Order
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.Meals, opt => opt.MapFrom(src => src.OrderMeals.Select(om => om.Meal)))
                .ForMember(dest => dest.TrackingNumber, opt => opt.MapFrom(src => src.Delivery != null ? src.Delivery.TrackingNumber : null));

            CreateMap<Delivery, OrderTrackingDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CorporateCompany, CorporateCompanyDto>()
               .ForMember(dest => dest.ActivePlanName,
                opt => opt.MapFrom(src => src.ActiveSubscriptionPlan != null ? src.ActiveSubscriptionPlan.PlanName : null));


            CreateMap<Meal, MealSummaryDto>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MealName))
              .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.MealCategory));


            CreateMap<OrderCreationDto, Order>(); // We'll customize this in service layer since it needs extra logic

            //Payment DTO <-> Payment Entity
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<Payment, PaymentResponseDto>()
            .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.SubscriptionPlan != null ? src.SubscriptionPlan.PlanName : null))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CorporateCompany != null ? src.CorporateCompany.CompanyName : null));


            CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.CompanyID, opt => opt.MapFrom(src => src.CompanyID));

            CreateMap<UpdateEmployeeDto, Employee>()
                .ForMember(dest => dest.EmployeeID, opt => opt.Ignore());

            CreateMap<DeliveryPersonRegisterDto, DeliveryPerson>();
            CreateMap<DeliveryPersonUpdateDto, DeliveryPerson>();

            CreateMap<DeliveryPerson, DeliveryPersonDto>()
                .ForMember(dest => dest.DeliveryPersonID, opt => opt.MapFrom(src => src.DeliveryPersonID))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.VehicleInfo, opt => opt.MapFrom(src => src.VehicleInfo));

            CreateMap<UpdateOrderStatusByChefDto, Order>()
             .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.OrderId))
             .ForMember(dest => dest.DeliveryStatus, opt => opt.MapFrom(src => Enum.Parse<DeliveryStatus>(src.NewStatus)))
             .ForMember(dest => dest.EmployeeID, opt => opt.Ignore())
             .ForMember(dest => dest.ChefID, opt => opt.Ignore())
             .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
             .ForMember(dest => dest.PaymentStatus, opt => opt.Ignore())
             .ForMember(dest => dest.OrderMeals, opt => opt.Ignore())
             .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
             .ForMember(dest => dest.ThirdPartyDeliveryService, opt => opt.Ignore())
             .ForMember(dest => dest.Payment, opt => opt.Ignore())
             .ForMember(dest => dest.Employee, opt => opt.Ignore())
             .ForMember(dest => dest.Chef, opt => opt.Ignore())
             .ForMember(dest => dest.Delivery, opt => opt.Ignore());

            CreateMap<UpdateMealAvailabilityDto, Meal>()
            .ForMember(dest => dest.MealID, opt => opt.MapFrom(src => src.MealId))
            .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => src.Availability))
            // Ignore all other fields to avoid accidental overwrites
            .ForMember(dest => dest.ChefID, opt => opt.Ignore())
            .ForMember(dest => dest.MealName, opt => opt.Ignore())
            .ForMember(dest => dest.MealDescription, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.Ignore())
            .ForMember(dest => dest.MealCategory, opt => opt.Ignore())
            .ForMember(dest => dest.PreparationTime, opt => opt.Ignore())
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Rating, opt => opt.Ignore())
            .ForMember(dest => dest.OrderMeals, opt => opt.Ignore())
            .ForMember(dest => dest.Chef, opt => opt.Ignore());

            CreateMap<DeliveryAssignDto, Delivery>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => DeliveryStatus.Assigned))
            .ForMember(dest => dest.PickedUpAt, opt => opt.MapFrom(_ => DateTime.UtcNow));


            CreateMap<Delivery, DeliveryDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<DietaryPreference, DietaryPreferenceDto>().ReverseMap();

            // Location mappings
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<CreateLocationDto, Location>();
            CreateMap<UpdateLocationDto, Location>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
