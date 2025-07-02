using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace MealTimes.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordResetController : ControllerBase
    {
        private readonly IPasswordResetService _passwordResetService;

        public PasswordResetController(IPasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        /// <summary>
        /// Send password reset email to user
        /// </summary>
        /// <param name="dto">Email address to send reset link to</param>
        /// <returns>Success message</returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _passwordResetService.SendPasswordResetEmailAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Reset password using token
        /// </summary>
        /// <param name="dto">Reset password data including token and new password</param>
        /// <returns>Success or error message</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _passwordResetService.ResetPasswordAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Validate password reset token
        /// </summary>
        /// <param name="token">Reset token</param>
        /// <param name="email">User email</param>
        /// <returns>Token validity status</returns>
        [HttpGet("validate-token")]
        public async Task<IActionResult> ValidateToken([FromQuery] string token, [FromQuery] string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return BadRequest("Token and email are required.");

            var result = await _passwordResetService.ValidateResetTokenAsync(token, email);
            return StatusCode(result.StatusCode, result);
        }
    }
}