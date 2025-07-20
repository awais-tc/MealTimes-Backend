using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Helpers;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace MealTimes.Service
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IHomeChefRepository _homeChefRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public LocationService(
            ILocationRepository locationRepository,
            IHomeChefRepository homeChefRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _locationRepository = locationRepository;
            _homeChefRepository = homeChefRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<GenericResponse<LocationDto>> CreateLocationAsync(CreateLocationDto dto)
        {
            var location = _mapper.Map<Location>(dto);
            var createdLocation = await _locationRepository.CreateLocationAsync(location);
            var locationDto = _mapper.Map<LocationDto>(createdLocation);

            return GenericResponse<LocationDto>.Success(locationDto, "Location created successfully.");
        }

        public async Task<GenericResponse<LocationDto>> UpdateLocationAsync(UpdateLocationDto dto)
        {
            var existingLocation = await _locationRepository.GetLocationByIdAsync(dto.LocationID);
            if (existingLocation == null)
                return GenericResponse<LocationDto>.Fail("Location not found.");

            _mapper.Map(dto, existingLocation);
            var updatedLocation = await _locationRepository.UpdateLocationAsync(existingLocation);
            var locationDto = _mapper.Map<LocationDto>(updatedLocation);

            return GenericResponse<LocationDto>.Success(locationDto, "Location updated successfully.");
        }

        public async Task<GenericResponse<LocationDto>> GetLocationByIdAsync(int locationId)
        {
            var location = await _locationRepository.GetLocationByIdAsync(locationId);
            if (location == null)
                return GenericResponse<LocationDto>.Fail("Location not found.");

            var locationDto = _mapper.Map<LocationDto>(location);
            return GenericResponse<LocationDto>.Success(locationDto);
        }

        public async Task<GenericResponse<bool>> DeleteLocationAsync(int locationId)
        {
            var result = await _locationRepository.DeleteLocationAsync(locationId);
            return result
                ? GenericResponse<bool>.Success(true, "Location deleted successfully.")
                : GenericResponse<bool>.Fail("Location not found or could not be deleted.");
        }

        public async Task<GenericResponse<GeocodeResponseDto>> GeocodeAddressAsync(string address)
        {
            try
            {
                // Using OpenStreetMap Nominatim API (free alternative to Google Maps)
                var encodedAddress = Uri.EscapeDataString(address);
                var url = $"https://nominatim.openstreetmap.org/search?format=json&q={encodedAddress}&limit=1";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "MealTimes/1.0");

                var response = await _httpClient.GetStringAsync(url);
                var results = JsonSerializer.Deserialize<List<NominatimResult>>(response);

                if (results == null || !results.Any())
                    return GenericResponse<GeocodeResponseDto>.Fail("Address not found.");

                var result = results.First();
                var geocodeResponse = new GeocodeResponseDto
                {
                    Latitude = double.Parse(result.lat),
                    Longitude = double.Parse(result.lon),
                    FormattedAddress = result.display_name,
                    City = ExtractAddressComponent(result.display_name, "city"),
                    State = ExtractAddressComponent(result.display_name, "state"),
                    PostalCode = ExtractAddressComponent(result.display_name, "postcode"),
                    Country = ExtractAddressComponent(result.display_name, "country")
                };

                return GenericResponse<GeocodeResponseDto>.Success(geocodeResponse);
            }
            catch (Exception ex)
            {
                return GenericResponse<GeocodeResponseDto>.Fail($"Geocoding failed: {ex.Message}");
            }
        }

        public async Task<GenericResponse<GeocodeResponseDto>> ReverseGeocodeAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "MealTimes/1.0");

                var response = await _httpClient.GetStringAsync(url);
                var result = JsonSerializer.Deserialize<NominatimResult>(response);

                if (result == null)
                    return GenericResponse<GeocodeResponseDto>.Fail("Location not found.");

                var geocodeResponse = new GeocodeResponseDto
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    FormattedAddress = result.display_name,
                    City = ExtractAddressComponent(result.display_name, "city"),
                    State = ExtractAddressComponent(result.display_name, "state"),
                    PostalCode = ExtractAddressComponent(result.display_name, "postcode"),
                    Country = ExtractAddressComponent(result.display_name, "country")
                };

                return GenericResponse<GeocodeResponseDto>.Success(geocodeResponse);
            }
            catch (Exception ex)
            {
                return GenericResponse<GeocodeResponseDto>.Fail($"Reverse geocoding failed: {ex.Message}");
            }
        }

        public async Task<GenericResponse<List<NearbyChefDto>>> GetNearbyChefs(LocationFilterDto filter)
        {
            var nearbyChefs = await _locationRepository.GetNearbyChefs(
                filter.Latitude, filter.Longitude, filter.RadiusKm);

            var nearbyChefDtos = nearbyChefs.Select(chef => new NearbyChefDto
            {
                ChefID = chef.ChefID,
                FullName = chef.FullName,
                Email = chef.Email,
                PhoneNumber = chef.PhoneNumber,
                Address = chef.Address,
                Rating = chef.Rating,
                DistanceKm = Math.Round(LocationHelper.CalculateDistance(
                    filter.Latitude, filter.Longitude,
                    chef.Location!.Latitude, chef.Location.Longitude), 2),
                Location = _mapper.Map<LocationDto>(chef.Location),
                AvailableMeals = _mapper.Map<List<MealDto>>(chef.Meals.Where(m => m.Availability))
            })
            .OrderBy(c => c.DistanceKm)
            .ToList();

            return GenericResponse<List<NearbyChefDto>>.Success(nearbyChefDtos);
        }

        public async Task<GenericResponse<List<MealDto>>> GetNearbyMeals(LocationFilterDto filter)
        {
            var nearbyMeals = await _locationRepository.GetNearbyMeals(
                filter.Latitude, filter.Longitude, filter.RadiusKm);

            var mealDtos = nearbyMeals.Select(meal =>
            {
                var dto = _mapper.Map<MealDto>(meal);
                // Add distance information
                dto.ChefName = meal.Chef.FullName;
                return dto;
            })
            .OrderBy(m => LocationHelper.CalculateDistance(
                filter.Latitude, filter.Longitude,
                nearbyMeals.First(meal => meal.MealID == m.MealID).Chef.Location!.Latitude,
                nearbyMeals.First(meal => meal.MealID == m.MealID).Chef.Location!.Longitude))
            .ToList();

            return GenericResponse<List<MealDto>>.Success(mealDtos);
        }

        public async Task<GenericResponse<double>> CalculateDeliveryDistance(int chefId, int employeeId)
        {
            var chef = await _homeChefRepository.GetByIdAsync(chefId);
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (chef?.Location == null)
                return GenericResponse<double>.Fail("Chef location not found.");

            if (employee?.Location == null)
                return GenericResponse<double>.Fail("Employee location not found.");

            var distance = LocationHelper.CalculateDistance(
                chef.Location.Latitude, chef.Location.Longitude,
                employee.Location.Latitude, employee.Location.Longitude);

            return GenericResponse<double>.Success(Math.Round(distance, 2));
        }

        public async Task<GenericResponse<bool>> AssignLocationToChef(int chefId, CreateLocationDto locationDto)
        {
            var location = _mapper.Map<Location>(locationDto);
            var createdLocation = await _locationRepository.CreateLocationAsync(location);

            var result = await _locationRepository.AssignLocationToChef(chefId, createdLocation.LocationID);
            return result
                ? GenericResponse<bool>.Success(true, "Location assigned to chef successfully.")
                : GenericResponse<bool>.Fail("Failed to assign location to chef.");
        }

        public async Task<GenericResponse<bool>> AssignLocationToCompany(int companyId, CreateLocationDto locationDto)
        {
            var location = _mapper.Map<Location>(locationDto);
            var createdLocation = await _locationRepository.CreateLocationAsync(location);

            var result = await _locationRepository.AssignLocationToCompany(companyId, createdLocation.LocationID);
            return result
                ? GenericResponse<bool>.Success(true, "Location assigned to company successfully.")
                : GenericResponse<bool>.Fail("Failed to assign location to company.");
        }

        public async Task<GenericResponse<bool>> AssignLocationToEmployee(int employeeId, CreateLocationDto locationDto)
        {
            var location = _mapper.Map<Location>(locationDto);
            var createdLocation = await _locationRepository.CreateLocationAsync(location);

            var result = await _locationRepository.AssignLocationToEmployee(employeeId, createdLocation.LocationID);
            return result
                ? GenericResponse<bool>.Success(true, "Location assigned to employee successfully.")
                : GenericResponse<bool>.Fail("Failed to assign location to employee.");
        }

        private string ExtractAddressComponent(string displayName, string component)
        {
            // Simple extraction logic - in production, you might want more sophisticated parsing
            var parts = displayName.Split(',');
            return component switch
            {
                "city" => parts.Length > 2 ? parts[1].Trim() : "",
                "state" => parts.Length > 3 ? parts[2].Trim() : "",
                "country" => parts.Length > 0 ? parts[^1].Trim() : "",
                _ => ""
            };
        }
    }

    // Helper class for Nominatim API response
    public class NominatimResult
    {
        public string lat { get; set; } = string.Empty;
        public string lon { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
    }
}