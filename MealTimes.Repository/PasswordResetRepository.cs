using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace MealTimes.Repository
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly AppDbContext _context;

        public PasswordResetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordResetToken> CreateTokenAsync(PasswordResetToken token)
        {
            await _context.PasswordResetTokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<PasswordResetToken?> GetValidTokenAsync(string token, string email)
        {
            return await _context.PasswordResetTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => 
                    t.Token == token && 
                    t.User.Email == email && 
                    !t.IsUsed && 
                    t.ExpiresAt > DateTime.UtcNow);
        }

        public async Task MarkTokenAsUsedAsync(int tokenId)
        {
            var token = await _context.PasswordResetTokens.FindAsync(tokenId);
            if (token != null)
            {
                token.IsUsed = true;
                token.UsedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteExpiredTokensAsync()
        {
            var expiredTokens = await _context.PasswordResetTokens
                .Where(t => t.ExpiresAt < DateTime.UtcNow || t.IsUsed)
                .ToListAsync();

            _context.PasswordResetTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasValidTokenAsync(int userId)
        {
            return await _context.PasswordResetTokens
                .AnyAsync(t => 
                    t.UserId == userId && 
                    !t.IsUsed && 
                    t.ExpiresAt > DateTime.UtcNow);
        }
    }
}