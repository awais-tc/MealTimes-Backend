using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace MealTimes.Service
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IPasswordResetRepository _passwordResetRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public PasswordResetService(
            IPasswordResetRepository passwordResetRepository,
            IUserRepository userRepository,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _passwordResetRepository = passwordResetRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<GenericResponse<PasswordResetResponseDto>> SendPasswordResetEmailAsync(ForgotPasswordDto dto)
        {
            try
            {
                // Find user by email
                var user = await _userRepository.GetUserByEmailAsync(dto.Email);
                if (user == null)
                {
                    // For security reasons, don't reveal if email exists or not
                    return GenericResponse<PasswordResetResponseDto>.Success(
                        new PasswordResetResponseDto 
                        { 
                            Message = "If an account with that email exists, a password reset link has been sent.",
                            IsSuccess = true 
                        });
                }

                // Check if user already has a valid token
                if (await _passwordResetRepository.HasValidTokenAsync(user.UserID))
                {
                    return GenericResponse<PasswordResetResponseDto>.Fail(
                        "A password reset email has already been sent. Please check your email or wait before requesting another.");
                }

                // Generate secure token
                var token = GenerateSecureToken();
                var expiresAt = DateTime.UtcNow.AddHours(1); // Token expires in 1 hour

                // Create password reset token
                var resetToken = new PasswordResetToken
                {
                    UserId = user.UserID,
                    Token = token,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = expiresAt,
                    IsUsed = false
                };

                await _passwordResetRepository.CreateTokenAsync(resetToken);

                // Generate reset link
                var baseUrl = _configuration["AppSettings:BaseUrl"] ?? "http://localhost:5173";
                var resetLink = $"{baseUrl}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(dto.Email)}";

                // Get user's name based on role
                var userName = GetUserDisplayName(user);

                // Send email
                var emailSent = await _emailService.SendPasswordResetEmailAsync(dto.Email, resetLink, userName);

                if (!emailSent)
                {
                    return GenericResponse<PasswordResetResponseDto>.Fail(
                        "Failed to send password reset email. Please try again later.");
                }

                // Clean up expired tokens
                await _passwordResetRepository.DeleteExpiredTokensAsync();

                return GenericResponse<PasswordResetResponseDto>.Success(
                    new PasswordResetResponseDto 
                    { 
                        Message = "Password reset email sent successfully. Please check your email.",
                        IsSuccess = true 
                    });
            }
            catch (Exception ex)
            {
                return GenericResponse<PasswordResetResponseDto>.Fail(
                    "An error occurred while processing your request. Please try again later.");
            }
        }

        public async Task<GenericResponse<PasswordResetResponseDto>> ResetPasswordAsync(ResetPasswordDto dto)
        {
            try
            {
                // Validate token
                var resetToken = await _passwordResetRepository.GetValidTokenAsync(dto.Token, dto.Email);
                if (resetToken == null)
                {
                    return GenericResponse<PasswordResetResponseDto>.Fail(
                        "Invalid or expired password reset token.");
                }

                // Hash new password
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

                // Update user password
                resetToken.User.PasswordHash = hashedPassword;
                await _userRepository.SaveChangesAsync();

                // Mark token as used
                await _passwordResetRepository.MarkTokenAsUsedAsync(resetToken.Id);

                return GenericResponse<PasswordResetResponseDto>.Success(
                    new PasswordResetResponseDto 
                    { 
                        Message = "Password reset successfully. You can now login with your new password.",
                        IsSuccess = true 
                    });
            }
            catch (Exception ex)
            {
                return GenericResponse<PasswordResetResponseDto>.Fail(
                    "An error occurred while resetting your password. Please try again.");
            }
        }

        public async Task<GenericResponse<bool>> ValidateResetTokenAsync(string token, string email)
        {
            try
            {
                var resetToken = await _passwordResetRepository.GetValidTokenAsync(token, email);
                return GenericResponse<bool>.Success(resetToken != null);
            }
            catch (Exception ex)
            {
                return GenericResponse<bool>.Fail("Error validating token.");
            }
        }

        private string GenerateSecureToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        private string GetUserDisplayName(User user)
        {
            return user.Role switch
            {
                "Admin" => user.Admin?.Email ?? user.Email,
                "Company" => user.CorporateCompany?.CompanyName ?? user.Email,
                "Employee" => user.Employee?.FullName ?? user.Email,
                "Chef" => user.HomeChef?.FullName ?? user.Email,
                "DeliveryPerson" => user.DeliveryPerson?.FullName ?? user.Email,
                _ => user.Email
            };
        }
    }
}