using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;

namespace MealTimes.Core.Service
{
    public interface ILocationService
    {
        Task<GenericResponse<LocationDto>> CreateLocationAsync(CreateLocationDto dto);
        Task<GenericResponse<LocationDto>> UpdateLocationAsync(UpdateLocationDto dto);
        Task<GenericResponse<LocationDto>> GetLocationByIdAsync(int locationId);
        Task<GenericResponse<bool>> DeleteLocationAsync(int locationId);

        // Geocoding services
        Task<GenericResponse<GeocodeResponseDto>> GeocodeAddressAsync(string address);
        Task<GenericResponse<GeocodeResponseDto>> ReverseGeocodeAsync(double latitude, double longitude);

        // Location-based queries
        Task<GenericResponse<List<NearbyChefDto>>> GetNearbyChefs(LocationFilterDto filter);
        Task<GenericResponse<List<MealDto>>> GetNearbyMeals(LocationFilterDto filter);
        Task<GenericResponse<double>> CalculateDeliveryDistance(int chefId, int employeeId);

        // Location assignment
        Task<GenericResponse<bool>> AssignLocationToChef(int chefId, CreateLocationDto locationDto);
        Task<GenericResponse<bool>> AssignLocationToCompany(int companyId, CreateLocationDto locationDto);
        Task<GenericResponse<bool>> AssignLocationToEmployee(int employeeId, CreateLocationDto locationDto);
    }
}