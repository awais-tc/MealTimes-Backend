using MealTimes.Core.DTOs;
using MealTimes.Core.Helpers;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace MealTimes.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _context;

        public LocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Location> CreateLocationAsync(Location location)
        {
            location.CreatedAt = DateTime.UtcNow;
            location.UpdatedAt = DateTime.UtcNow;
            
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();
            return location;
        }

        public async Task<Location?> GetLocationByIdAsync(int locationId)
        {
            return await _context.Locations.FindAsync(locationId);
        }

        public async Task<Location> UpdateLocationAsync(Location location)
        {
            location.UpdatedAt = DateTime.UtcNow;
            _context.Locations.Update(location);
            await _context.SaveChangesAsync();
            return location;
        }

        public async Task<bool> DeleteLocationAsync(int locationId)
        {
            var location = await GetLocationByIdAsync(locationId);
            if (location == null) return false;

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<HomeChef>> GetNearbyChefs(double latitude, double longitude, double radiusKm)
        {
            // Calculate bounding box for efficient querying
            var (minLat, maxLat, minLon, maxLon) = LocationHelper.GetBoundingBox(latitude, longitude, radiusKm);

            var chefs = await _context.HomeChefs
                .Include(c => c.Location)
                .Include(c => c.Meals.Where(m => m.Availability))
                .Where(c => c.Location != null &&
                           c.Location.Latitude >= minLat && c.Location.Latitude <= maxLat &&
                           c.Location.Longitude >= minLon && c.Location.Longitude <= maxLon)
                .ToListAsync();

            // Filter by exact distance using Haversine formula
            return chefs.Where(c => 
                LocationHelper.IsWithinRadius(latitude, longitude, 
                    c.Location!.Latitude, c.Location.Longitude, radiusKm))
                .ToList();
        }

        public async Task<List<Meal>> GetNearbyMeals(double latitude, double longitude, double radiusKm)
        {
            var (minLat, maxLat, minLon, maxLon) = LocationHelper.GetBoundingBox(latitude, longitude, radiusKm);

            var meals = await _context.Meals
                .Include(m => m.Chef)
                .ThenInclude(c => c.Location)
                .Where(m => m.Availability &&
                           m.Chef.Location != null &&
                           m.Chef.Location.Latitude >= minLat && m.Chef.Location.Latitude <= maxLat &&
                           m.Chef.Location.Longitude >= minLon && m.Chef.Location.Longitude <= maxLon)
                .ToListAsync();

            return meals.Where(m => 
                LocationHelper.IsWithinRadius(latitude, longitude,
                    m.Chef.Location!.Latitude, m.Chef.Location.Longitude, radiusKm))
                .ToList();
        }

        public async Task<bool> AssignLocationToChef(int chefId, int locationId)
        {
            var chef = await _context.HomeChefs.FindAsync(chefId);
            if (chef == null) return false;

            chef.LocationID = locationId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignLocationToCompany(int companyId, int locationId)
        {
            var company = await _context.CorporateCompanies.FindAsync(companyId);
            if (company == null) return false;

            company.LocationID = locationId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignLocationToEmployee(int employeeId, int locationId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null) return false;

            employee.LocationID = locationId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}