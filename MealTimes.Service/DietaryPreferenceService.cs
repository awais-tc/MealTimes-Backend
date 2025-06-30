using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;

namespace MealTimes.Service
{
    public class DietaryPreferenceService : IDietaryPreferenceService
    {
        private readonly IDietaryPreferenceRepository _repository;
        private readonly IMapper _mapper;

        public DietaryPreferenceService(IDietaryPreferenceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GenericResponse<DietaryPreferenceDto>> GetByEmployeeIdAsync(int employeeId)
        {
            var preference = await _repository.GetByEmployeeIdAsync(employeeId);
            if (preference is null)
                return GenericResponse<DietaryPreferenceDto>.Fail("No dietary preference found.");

            var dto = _mapper.Map<DietaryPreferenceDto>(preference);
            return GenericResponse<DietaryPreferenceDto>.Success(dto);
        }

        public async Task<GenericResponse<string>> UpsertAsync(int employeeId, DietaryPreferenceDto dto)
        {
            await _repository.UpsertAsync(employeeId, dto);
            return GenericResponse<string>.Success("Dietary preferences saved successfully.");
        }
    }
}
