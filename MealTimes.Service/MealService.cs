using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Core.Responses;
using MealTimes.Core.Service;

namespace MealTimes.Service;

public class MealService : IMealService
{
    private readonly IMealRepository _mealRepo;
    private readonly IMapper _mapper;

    public MealService(IMealRepository mealRepo, IMapper mapper)
    {
        _mealRepo = mealRepo;
        _mapper = mapper;
    }

    public async Task<GenericResponse<MealDto>> CreateMealAsync(MealCreateDto dto)
    {
        var meal = _mapper.Map<Meal>(dto);

        await _mealRepo.AddMealAsync(meal);
        await _mealRepo.SaveChangesAsync();

        var result = _mapper.Map<MealDto>(meal);
        return GenericResponse<MealDto>.Success(result, "Meal created successfully");
    }

    public async Task<GenericResponse<MealDto>> UpdateMealAsync(MealUpdateDto dto)
    {
        var meal = await _mealRepo.GetMealByIdAsync(dto.MealID);
        if (meal == null)
            return GenericResponse<MealDto>.Fail("Meal not found");

        _mapper.Map(dto, meal);
        await _mealRepo.UpdateMealAsync(meal);
        await _mealRepo.SaveChangesAsync();

        var result = _mapper.Map<MealDto>(meal);
        return GenericResponse<MealDto>.Success(result, "Meal updated successfully");
    }

    public async Task<GenericResponse<bool>> DeleteMealAsync(int mealId)
    {
        var meal = await _mealRepo.GetMealByIdAsync(mealId);
        if (meal == null)
            return GenericResponse<bool>.Fail("Meal not found");

        await _mealRepo.DeleteMealAsync(meal);
        await _mealRepo.SaveChangesAsync();
        return GenericResponse<bool>.Success(true, "Meal deleted successfully");
    }

    public async Task<GenericResponse<MealDto>> GetMealByIdAsync(int id)
    {
        var meal = await _mealRepo.GetMealByIdAsync(id);
        if (meal == null)
            return GenericResponse<MealDto>.Fail("Meal not found");

        var dto = _mapper.Map<MealDto>(meal);
        return GenericResponse<MealDto>.Success(dto);
    }

    public async Task<GenericResponse<IEnumerable<MealDto>>> GetAllMealsAsync()
    {
        var meals = await _mealRepo.GetAllMealsAsync();
        var dto = _mapper.Map<IEnumerable<MealDto>>(meals);
        return GenericResponse<IEnumerable<MealDto>>.Success(dto);
    }

    public async Task<GenericResponse<IEnumerable<MealDto>>> GetMealsByChefAsync(int chefId)
    {
        var meals = await _mealRepo.GetMealsByChefAsync(chefId);
        var dto = _mapper.Map<IEnumerable<MealDto>>(meals);
        return GenericResponse<IEnumerable<MealDto>>.Success(dto);
    }

    public async Task<GenericResponse<IEnumerable<MealDto>>> FilterMealsAsync(MealFilterDto filter)
    {
        var meals = await _mealRepo.FilterMealsAsync(filter);
        var dto = _mapper.Map<IEnumerable<MealDto>>(meals);
        return GenericResponse<IEnumerable<MealDto>>.Success(dto);
    }

    public async Task<GenericResponse<IEnumerable<MealDto>>> GetTopRatedMealsAsync(int count = 5)
    {
        var meals = await _mealRepo.GetTopRatedMealsAsync(count);
        var dto = _mapper.Map<IEnumerable<MealDto>>(meals);
        return GenericResponse<IEnumerable<MealDto>>.Success(dto);
    }
}
