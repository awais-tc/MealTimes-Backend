using MealTimes.Core.DTOs;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _companyService.GetByIdAsync(id);
            if (!response.IsSuccess)
                return NotFound(response);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCorporateCompanyDto dto)
        {
            if (id != dto.CompanyID)
                return BadRequest("Mismatched CompanyID");

            var response = await _companyService.UpdateAsync(dto);
            if (!response.IsSuccess)
                return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _companyService.DeleteAsync(id);
            if (!response.IsSuccess)
                return NotFound(response);
            return Ok(response);
        }
    }
}
