using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Service
{
    public class DeliveryPersonService : IDeliveryPersonService
    {
        private readonly IDeliveryPersonRepository _repository;
        private readonly IMapper _mapper;

        public DeliveryPersonService(IDeliveryPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GenericResponse<IEnumerable<DeliveryPersonDto>>> GetAllAsync()
        {
            var persons = await _repository.GetAllAsync();
            return GenericResponse<IEnumerable<DeliveryPersonDto>>.Success(_mapper.Map<IEnumerable<DeliveryPersonDto>>(persons));
        }

        public async Task<GenericResponse<DeliveryPersonDto>> GetByIdAsync(int id)
        {
            var person = await _repository.GetByIdAsync(id);
            return person == null
                ? GenericResponse<DeliveryPersonDto>.Fail("Delivery person not found.")
                : GenericResponse<DeliveryPersonDto>.Success(_mapper.Map<DeliveryPersonDto>(person));
        }

        public async Task<GenericResponse<DeliveryPersonDto>> UpdateAsync(DeliveryPersonUpdateDto dto)
        {
            var person = await _repository.GetByIdAsync(dto.DeliveryPersonID);
            if (person == null)
                return GenericResponse<DeliveryPersonDto>.Fail("Delivery person not found.");

            if (!string.IsNullOrWhiteSpace(dto.FullName)) person.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber)) person.PhoneNumber = dto.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(dto.Address)) person.Address = dto.Address;
            if (!string.IsNullOrWhiteSpace(dto.VehicleInfo)) person.VehicleInfo = dto.VehicleInfo;

            await _repository.UpdateAsync(person);
            await _repository.SaveChangesAsync();

            return GenericResponse<DeliveryPersonDto>.Success(_mapper.Map<DeliveryPersonDto>(person), "Updated successfully.");
        }
    }
}
