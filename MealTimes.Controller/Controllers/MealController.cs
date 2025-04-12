using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace MealTimes.Controller.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealController : ControllerBase
{
    private readonly IMealService _mealService;

    public MealController(IMealService mealService)
    {
        _mealService = mealService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMeal([FromBody] MealCreateDto dto)
    {
        var response = await _mealService.CreateMealAsync(dto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeal(int id, [FromBody] MealUpdateDto dto)
    {
        if (id != dto.MealID)
            return BadRequest("Meal ID mismatch");

        var response = await _mealService.UpdateMealAsync(dto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeal(int id)
    {
        var response = await _mealService.DeleteMealAsync(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMealById(int id)
    {
        var response = await _mealService.GetMealByIdAsync(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllMeals()
    {
        var response = await _mealService.GetAllMealsAsync();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("chef/{chefId}")]
    public async Task<IActionResult> GetMealsByChef(int chefId)
    {
        var response = await _mealService.GetMealsByChefAsync(chefId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("filter")]
    public async Task<IActionResult> FilterMeals([FromBody] MealFilterDto filter)
    {
        var response = await _mealService.FilterMealsAsync(filter);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("top-rated")]
    public async Task<IActionResult> GetTopRatedMeals([FromQuery] int count = 5)
    {
        var response = await _mealService.GetTopRatedMealsAsync(count);
        return StatusCode(response.StatusCode, response);
    }
}
