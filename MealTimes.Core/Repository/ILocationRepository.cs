using MealTimes.Core.DTOs;
using MealTimes.Core.Models;

namespace MealTimes.Core.Repository
{
    public interface ILocationRepository
    {
        Task<Location> CreateLocationAsync(Location location);
        Task<Location?> GetLocationByIdAsync(int locationId);
        Task<Location> UpdateLocationAsync(Location location);
        Task<bool> DeleteLocationAsync(int locationId);

        // Spatial queries
        Task<List<HomeChef>> GetNearbyChefs(double latitude, double longitude, double radiusKm);
        Task<List<Meal>> GetNearbyMeals(double latitude, double longitude, double radiusKm);

        // Location assignments
        Task<bool> AssignLocationToChef(int chefId, int locationId);
        Task<bool> AssignLocationToCompany(int companyId, int locationId);
        Task<bool> AssignLocationToEmployee(int employeeId, int locationId);

        Task SaveChangesAsync();
    }
}