using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MealTimes.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Create a new location
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationDto dto)
        {
            var response = await _locationService.CreateLocationAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Update an existing location
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] UpdateLocationDto dto)
        {
            if (id != dto.LocationID)
                return BadRequest("Location ID mismatch");

            var response = await _locationService.UpdateLocationAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get location by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var response = await _locationService.GetLocationByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Delete a location
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var response = await _locationService.DeleteLocationAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Geocode an address to get coordinates
        /// </summary>
        [HttpPost("geocode")]
        public async Task<IActionResult> GeocodeAddress([FromBody] GeocodeRequestDto dto)
        {
            var response = await _locationService.GeocodeAddressAsync(dto.Address);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Reverse geocode coordinates to get address
        /// </summary>
        [HttpGet("reverse-geocode")]
        public async Task<IActionResult> ReverseGeocode([FromQuery] double latitude, [FromQuery] double longitude)
        {
            var response = await _locationService.ReverseGeocodeAsync(latitude, longitude);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get nearby chefs within specified radius
        /// </summary>
        [HttpPost("nearby-chefs")]
        public async Task<IActionResult> GetNearbyChefs([FromBody] LocationFilterDto filter)
        {
            var response = await _locationService.GetNearbyChefs(filter);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get nearby meals within specified radius
        /// </summary>
        [HttpPost("nearby-meals")]
        public async Task<IActionResult> GetNearbyMeals([FromBody] LocationFilterDto filter)
        {
            var response = await _locationService.GetNearbyMeals(filter);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Calculate delivery distance between chef and employee
        /// </summary>
        [HttpGet("delivery-distance")]
        public async Task<IActionResult> CalculateDeliveryDistance([FromQuery] int chefId, [FromQuery] int employeeId)
        {
            var response = await _locationService.CalculateDeliveryDistance(chefId, employeeId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Assign location to a chef
        /// </summary>
        [HttpPost("assign/chef/{chefId}")]
        [Authorize(Roles = "Chef,Admin")]
        public async Task<IActionResult> AssignLocationToChef(int chefId, [FromBody] CreateLocationDto dto)
        {
            var response = await _locationService.AssignLocationToChef(chefId, dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Assign location to a company
        /// </summary>
        [HttpPost("assign/company/{companyId}")]
        [Authorize(Roles = "Company,Admin")]
        public async Task<IActionResult> AssignLocationToCompany(int companyId, [FromBody] CreateLocationDto dto)
        {
            var response = await _locationService.AssignLocationToCompany(companyId, dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Assign location to an employee
        /// </summary>
        [HttpPost("assign/employee/{employeeId}")]
        [Authorize(Roles = "Employee,Company,Admin")]
        public async Task<IActionResult> AssignLocationToEmployee(int employeeId, [FromBody] CreateLocationDto dto)
        {
            var response = await _locationService.AssignLocationToEmployee(employeeId, dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}