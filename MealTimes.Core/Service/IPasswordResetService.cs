using MealTimes.Core.DTOs;
using MealTimes.Core.Responses;

namespace MealTimes.Core.Service
{
    public interface IPasswordResetService
    {
        Task<GenericResponse<PasswordResetResponseDto>> SendPasswordResetEmailAsync(ForgotPasswordDto dto);
        Task<GenericResponse<PasswordResetResponseDto>> ResetPasswordAsync(ResetPasswordDto dto);
        Task<GenericResponse<bool>> ValidateResetTokenAsync(string token, string email);
    }
}