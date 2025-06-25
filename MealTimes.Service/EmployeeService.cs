using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;

namespace MealTimes.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<GenericResponse<List<EmployeeDto>>> GetEmployeesByCompanyIdAsync(int companyId)
        {
            var employees = await _employeeRepository.GetEmployeesByCompanyIdAsync(companyId);

            var dtos = employees.Select(e =>
            {
                var dto = _mapper.Map<EmployeeDto>(e);
                dto.CompanyName = e.CorporateCompany?.CompanyName;
                return dto;
            }).ToList();

            return new GenericResponse<List<EmployeeDto>>
            {
                Data = dtos,
                IsSuccess = true,
                Message = "Employees fetched successfully"
            };
        }

        public async Task<GenericResponse<EmployeeDto>> GetByIdAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
            {
                return new GenericResponse<EmployeeDto>
                {
                    IsSuccess = false,
                    Message = "Employee not found"
                };
            }

            var dto = _mapper.Map<EmployeeDto>(employee);
            dto.CompanyName = employee.CorporateCompany?.CompanyName;

            return new GenericResponse<EmployeeDto>
            {
                Data = dto,
                IsSuccess = true,
                Message = "Employee fetched successfully"
            };
        }

        public async Task<GenericResponse<EmployeeDto>> UpdateAsync(UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeID);
            if (employee == null)
            {
                return new GenericResponse<EmployeeDto>
                {
                    IsSuccess = false,
                    Message = "Employee not found"
                };
            }

            // Map updated fields
            employee.FullName = dto.FullName;
            employee.Email = dto.Email;

            await _employeeRepository.UpdateAsync(employee);

            var updatedDto = _mapper.Map<EmployeeDto>(employee);
            return new GenericResponse<EmployeeDto>
            {
                Data = updatedDto,
                IsSuccess = true,
                Message = "Employee updated successfully"
            };
        }
    }
}
