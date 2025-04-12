using MealTimes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.Repository
{
    public interface ICorporateCompanyRepository
    {
        Task<CorporateCompany> GetByIdAsync(int id);
        Task<List<CorporateCompany>> GetAllCompaniesAsync();
        Task AddAsync(CorporateCompany corporateCompany);
        Task UpdateAsync(CorporateCompany corporateCompany);
        Task DeleteAsync(int id);
    }
}
