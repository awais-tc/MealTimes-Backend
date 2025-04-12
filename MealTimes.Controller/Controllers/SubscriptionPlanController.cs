using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MealTimes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        // POST: api/MealPlan
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMealPlan([FromBody] SubscriptionPlanCreateDto createDto)
        {
            var response = await _subscriptionPlanService.CreateSubscriptionPlanAsync(createDto);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/MealPlan
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMealPlans()
        {
            var response = await _subscriptionPlanService.GetAllPlansAsync();
            return Ok(response);
        }

        // GET: api/MealPlan/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMealPlanById(int id)
        {
            var response = await _subscriptionPlanService.GetPlanByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // PUT: api/MealPlan/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMealPlan(int id, [FromBody] SubscriptionPlanUpdateDto updateDto)
        {
            var response = await _subscriptionPlanService.UpdateSubscriptionPlanAsync(id, updateDto);
            return StatusCode(response.StatusCode, response);
        }

        // DELETE: api/MealPlan/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMealPlan(int id)
        {
            var response = await _subscriptionPlanService.DeleteSubscriptionPlanAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
