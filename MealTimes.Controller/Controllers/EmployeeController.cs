using MealTimes.Core.DTOs;
using MealTimes.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MealTimes.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyId(int companyId)
        {
            var response = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _employeeService.GetByIdAsync(id);
            if (!response.IsSuccess)
                return NotFound(response);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
        {
            if (id != dto.EmployeeID)
                return BadRequest("ID mismatch");

            var response = await _employeeService.UpdateAsync(dto);
            if (!response.IsSuccess)
                return NotFound(response);
            return Ok(response);
        }
    }
}
