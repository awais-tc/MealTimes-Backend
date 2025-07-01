using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using Moq;

namespace MealTimes.Service.Tests
{
    public class SubscriptionPlanServiceTests
    {
        private readonly Mock<ISubscriptionPlanRepository> _planRepoMock = new();
        private readonly Mock<ICorporateCompanyRepository> _companyRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ISubscriptionHistoryRepository> _historyRepoMock = new();

        private readonly SubscriptionPlanService _service;

        public SubscriptionPlanServiceTests()
        {
            _service = new SubscriptionPlanService(
                _planRepoMock.Object,
                _companyRepoMock.Object,
                _mapperMock.Object,
                _historyRepoMock.Object
            );
        }

        [Fact]
        public async Task CreateSubscriptionPlanAsync_ShouldReturnSuccess_WhenPlanIsNew()
        {
            // Arrange
            var dto = new SubscriptionPlanCreateDto
            {
                PlanName = "Gold Plan",
                Price = 999,
                MealLimitPerDay = 3,
                DurationInDays = 30,
                IsCustomizable = false,
                MaxEmployees = 50
            };

            var newPlan = new SubscriptionPlan
            {
                SubscriptionPlanID = 1,
                PlanName = dto.PlanName,
                Price = dto.Price,
                MealLimitPerDay = dto.MealLimitPerDay,
                DurationInDays = dto.DurationInDays,
                IsCustomizable = dto.IsCustomizable,
                MaxEmployees = dto.MaxEmployees
            };

            var mappedDto = new SubscriptionPlanDto
            {
                SubscriptionPlanID = 1,
                PlanName = dto.PlanName,
                Price = dto.Price,
                MealLimitPerDay = dto.MealLimitPerDay,
                DurationInDays = dto.DurationInDays,
                IsCustomizable = dto.IsCustomizable,
                MaxEmployees = dto.MaxEmployees
            };

            _planRepoMock.Setup(r => r.GetByNameAsync(dto.PlanName)).ReturnsAsync((SubscriptionPlan)null!);
            _mapperMock.Setup(m => m.Map<SubscriptionPlan>(dto)).Returns(newPlan);
            _planRepoMock.Setup(r => r.AddSubscriptionPlanAsync(newPlan)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<SubscriptionPlanDto>(newPlan)).Returns(mappedDto);

            // Act
            var result = await _service.CreateSubscriptionPlanAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Subscription plan created successfully.", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(dto.PlanName, result.Data.PlanName);

            _planRepoMock.Verify(r => r.GetByNameAsync(dto.PlanName), Times.Once);
            _planRepoMock.Verify(r => r.AddSubscriptionPlanAsync(newPlan), Times.Once);
            _mapperMock.Verify(m => m.Map<SubscriptionPlan>(dto), Times.Once);
            _mapperMock.Verify(m => m.Map<SubscriptionPlanDto>(newPlan), Times.Once);
        }

        [Fact]
        public async Task CreateSubscriptionPlanAsync_ShouldFail_WhenPlanAlreadyExists()
        {
            // Arrange
            var dto = new SubscriptionPlanCreateDto { PlanName = "Basic Plan" };

            var existingPlan = new SubscriptionPlan
            {
                SubscriptionPlanID = 1,
                PlanName = dto.PlanName
            };

            _planRepoMock.Setup(r => r.GetByNameAsync(dto.PlanName)).ReturnsAsync(existingPlan);

            // Act
            var result = await _service.CreateSubscriptionPlanAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("A plan with this name already exists.", result.Message);
            Assert.Null(result.Data);

            _planRepoMock.Verify(r => r.GetByNameAsync(dto.PlanName), Times.Once);
            _planRepoMock.Verify(r => r.AddSubscriptionPlanAsync(It.IsAny<SubscriptionPlan>()), Times.Never);
        }
    }
}
