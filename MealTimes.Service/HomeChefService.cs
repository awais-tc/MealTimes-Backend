using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;

namespace MealTimes.Service
{
    public class HomeChefService : IHomeChefService
    {
        private readonly IHomeChefRepository _homeChefRepository;
        private readonly IMapper _mapper;

        public HomeChefService(IHomeChefRepository homeChefRepository, IMapper mapper)
        {
            _homeChefRepository = homeChefRepository;
            _mapper = mapper;
        }

        public async Task<GenericResponse<HomeChefDto>> GetByIdAsync(int chefId)
        {
            var chef = await _homeChefRepository.GetByIdAsync(chefId);
            if (chef == null)
            {
                return GenericResponse<HomeChefDto>.Fail("Chef not found", 404);
            }

            var chefDto = _mapper.Map<HomeChefDto>(chef);
            return GenericResponse<HomeChefDto>.Success(chefDto);
        }

    }
}
