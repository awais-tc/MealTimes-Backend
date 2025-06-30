using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace MealTimes.Repository
{
    public class DietaryPreferenceRepository : IDietaryPreferenceRepository
    {
        private readonly AppDbContext _context;

        public DietaryPreferenceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DietaryPreference?> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.DietaryPreferences.FirstOrDefaultAsync(d => d.EmployeeID == employeeId);
        }

        public async Task<DietaryPreference> UpsertAsync(int employeeId, DietaryPreferenceDto dto)
        {
            var existing = await GetByEmployeeIdAsync(employeeId);

            if (existing is null)
            {
                var newPref = new DietaryPreference
                {
                    EmployeeID = employeeId,
                    Allergies = dto.Allergies,
                    Preferences = dto.Preferences,
                    Restrictions = dto.Restrictions,
                    CustomNotes = dto.CustomNotes
                };
                _context.DietaryPreferences.Add(newPref);
                await _context.SaveChangesAsync();
                return newPref;
            }

            existing.Allergies = dto.Allergies;
            existing.Preferences = dto.Preferences;
            existing.Restrictions = dto.Restrictions;
            existing.CustomNotes = dto.CustomNotes;

            await _context.SaveChangesAsync();
            return existing;
        }
    }

}
