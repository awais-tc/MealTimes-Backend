using MealTimes.Core.Models;
using MealTimes.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MealTimes.Core.Repository
{
    public class CorporateCompanyRepository : ICorporateCompanyRepository
    {
        private readonly AppDbContext _context;

        public CorporateCompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CorporateCompany> GetByIdAsync(int id)
        {
            return await _context.CorporateCompanies.FindAsync(id);
        }

        public async Task<List<CorporateCompany>> GetAllCompaniesAsync()
        {
            return await _context.CorporateCompanies.ToListAsync();
        }

        public async Task AddAsync(CorporateCompany corporateCompany)
        {
            await _context.CorporateCompanies.AddAsync(corporateCompany);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CorporateCompany corporateCompany)
        {
            _context.CorporateCompanies.Update(corporateCompany);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _context.CorporateCompanies.FindAsync(id);
            if (company != null)
            {
                _context.CorporateCompanies.Remove(company);
                await _context.SaveChangesAsync();
            }
        }
    }
}
