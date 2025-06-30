using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MealTimes.Controller.Controllers
{
    [ApiController]
    [Route("api/dietary-preferences")]
    public class DietaryPreferencesController : ControllerBase
    {
        private readonly IDietaryPreferenceService _service;

        public DietaryPreferencesController(IDietaryPreferenceService service)
        {
            _service = service;
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> Get(int employeeId)
        {
            var response = await _service.GetByEmployeeIdAsync(employeeId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("{employeeId}")]
        public async Task<IActionResult> Save(int employeeId, [FromBody] DietaryPreferenceDto dto)
        {
            var response = await _service.UpsertAsync(employeeId, dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
