using MealTimes.Core.Models;

namespace MealTimes.Core.Repository
{
    public interface IPasswordResetRepository
    {
        Task<PasswordResetToken> CreateTokenAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetValidTokenAsync(string token, string email);
        Task MarkTokenAsUsedAsync(int tokenId);
        Task DeleteExpiredTokensAsync();
        Task<bool> HasValidTokenAsync(int userId);
    }
}