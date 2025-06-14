using MealTimes.Core.Service;
using MealTimes.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MealTimes.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorporateCompanyController : ControllerBase
    {
        private readonly ICorporateCompanyService _companyService;

        public CorporateCompanyController(ICorporateCompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var response = await _companyService.GetAllCompaniesAsync();
            return Ok(response);
        }
    }
}
