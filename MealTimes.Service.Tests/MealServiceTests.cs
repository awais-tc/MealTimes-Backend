using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using Moq;

namespace MealTimes.Service.Tests
{
    public class MealServiceTests
    {
        private readonly Mock<IMealRepository> _mealRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly MealService _mealService;

        public MealServiceTests()
        {
            _mealService = new MealService(_mealRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateMealAsync_ShouldReturnSuccess_WhenMealIsCreated()
        {
            // Arrange
            var dto = new MealCreateDto
            {
                ChefID = 1,
                MealName = "Test Meal",
                MealDescription = "Delicious",
                Price = 10.99m,
                MealCategory = "Lunch",
                PreparationTime = 20,
                ImageUrl = null,
                Availability = true
            };

            var mealEntity = new Meal
            {
                MealID = 0,
                ChefID = dto.ChefID,
                MealName = dto.MealName,
                MealDescription = dto.MealDescription,
                Price = dto.Price,
                MealCategory = dto.MealCategory,
                PreparationTime = dto.PreparationTime,
                ImageUrl = dto.ImageUrl,
                Availability = dto.Availability
            };

            var mealDto = new MealDto
            {
                MealID = 1,
                MealName = "Test Meal",
                MealDescription = "Delicious",
                Price = 10.99m,
                MealCategory = "Lunch",
                PreparationTime = 20,
                ImageUrl = null,
                Availability = true,
                ChefName = "Chef A",
                Rating = 0
            };

            _mapperMock.Setup(m => m.Map<Meal>(dto)).Returns(mealEntity);
            _mealRepoMock.Setup(r => r.AddMealAsync(mealEntity)).Returns(Task.CompletedTask);
            _mealRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<MealDto>(mealEntity)).Returns(mealDto);

            // Act
            var result = await _mealService.CreateMealAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Meal created successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(mealDto.MealName, result.Data.MealName);
            Assert.Equal(mealDto.Price, result.Data.Price);

            _mapperMock.Verify(m => m.Map<Meal>(dto), Times.Once);
            _mealRepoMock.Verify(r => r.AddMealAsync(mealEntity), Times.Once);
            _mealRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<MealDto>(mealEntity), Times.Once);
        }
    }
}
