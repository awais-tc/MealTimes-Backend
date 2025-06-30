using Microsoft.AspNetCore.Mvc;
using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
using MealTimes.Core.Responses;
using Microsoft.AspNetCore.Authorization;
using MealTimes.Core.Models;

namespace MealTimes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterDto dto)
        {
            var response = await _userService.RegisterAdminAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("register/company")]
        public async Task<IActionResult> RegisterCompany([FromBody] CorporateCompanyRegisterDto dto)
        {
            var response = await _userService.RegisterCompanyAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("register/employee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeRegisterDto dto)
        {
            var response = await _userService.RegisterEmployeeAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("register/chef")]
        public async Task<IActionResult> RegisterChef([FromBody] HomeChefRegisterDto dto)
        {
            var response = await _userService.RegisterHomeChefAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("register/delivery-person")]
        public async Task<IActionResult> RegisterDeliveryPerson([FromBody] DeliveryPersonRegisterDto dto)
        {
            var response = await _userService.RegisterDeliveryPersonAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _userService.LoginAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsersAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUserAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst("UserID")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid token or user ID missing." });

            var response = await _userService.GetUserByIdAsync(userId);

            // Wrap the returned DTO in "userDto" if successful
            if (response.IsSuccess && response.Data != null)
            {
                return Ok(new
                {
                    isSuccess = true,
                    message = response.Message,
                    data = new
                    {
                        userDto = response.Data
                    },
                    statusCode = 200
                });
            }

            return StatusCode(response.StatusCode, response);
        }
    }
}
