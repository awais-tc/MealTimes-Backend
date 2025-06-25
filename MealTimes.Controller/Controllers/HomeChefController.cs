using MealTimes.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MealTimes.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeChefController : ControllerBase
    {
        private readonly IHomeChefService _homeChefService;

        public HomeChefController(IHomeChefService homeChefService)
        {
            _homeChefService = homeChefService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChefById(int id)
        {
            var response = await _homeChefService.GetByIdAsync(id);
            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response);
        }
    }
}
