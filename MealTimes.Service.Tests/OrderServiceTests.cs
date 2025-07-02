using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace MealTimes.Service.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepoMock = new();
        private readonly Mock<IEmployeeRepository> _employeeRepoMock = new();
        private readonly Mock<IMealRepository> _mealRepoMock = new();
        private readonly Mock<ICorporateCompanyRepository> _companyRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly DeliveryRepository _deliveryRepo;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new AppDbContext(options);
            _deliveryRepo = new DeliveryRepository(context);

            _orderService = new OrderService(
                _orderRepoMock.Object,
                _employeeRepoMock.Object,
                _mealRepoMock.Object,
                _deliveryRepo, 
                _companyRepoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateOrder_WhenInputIsValid()
        {
            // Arrange
            var employeeId = 1;
            var chefId = 10;
            var today = DateTime.UtcNow.Date;

            var mealIds = new List<int> { 101, 102 };
            var dto = new CreateOrderDto
            {
                EmployeeID = employeeId,
                Meals = mealIds.Select(id => new MealOrderDto { MealID = id }).ToList()
            };

            var subscriptionPlan = new SubscriptionPlan
            {
                MealLimitPerDay = 5,
                MaxEmployees = 10
            };

            var company = new CorporateCompany
            {
                CompanyID = 1,
                CompanyName = "Test Corp",               // ✅ Required
                Email = "test@corp.com",                 // ✅ Required
                Address = "123 Test Street",             // ✅ Required
                PlanStartDate = today.AddDays(-1),
                PlanEndDate = today.AddDays(1),
                ActiveSubscriptionPlan = subscriptionPlan,
                Employees = new List<Employee> { new Employee { EmployeeID = employeeId } },
                UserID = 1,
                User = new User()
            };


            var employee = new Employee
            {
                EmployeeID = employeeId,
                CorporateCompany = company
            };

            var existingOrders = new List<Order>(); // No previous orders

            var meals = mealIds.Select(id => new Meal
            {
                MealID = id,
                ChefID = chefId
            }).ToList();

            var orderToReturn = new Order
            {
                OrderID = 99,
                EmployeeID = employeeId,
                ChefID = chefId,
                OrderDate = DateTime.UtcNow,
                DeliveryStatus = DeliveryStatus.Pending,
                PaymentStatus = PaymentStatus.Succeeded,
                OrderMeals = meals.Select(m => new OrderMeal
                {
                    MealID = m.MealID,
                    Quantity = 1
                }).ToList()
            };

            var mappedDto = new OrderResponseDto
            {
                OrderID = 99,
                EmployeeID = employeeId,
                ChefID = chefId,
                OrderDate = orderToReturn.OrderDate,
                DeliveryStatus = DeliveryStatus.Pending.ToString(),
                PaymentStatus = PaymentStatus.Succeeded.ToString(),
                Meals = new List<MealSummaryDto>()
            };

            _employeeRepoMock.Setup(x => x.GetEmployeeWithCompanyAsync(employeeId)).ReturnsAsync(employee);
            _orderRepoMock.Setup(x => x.GetOrdersByEmployeeAsync(employeeId)).ReturnsAsync(existingOrders);
            _mealRepoMock.Setup(x => x.GetMealsByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(meals);
            _orderRepoMock.Setup(x => x.CreateOrderAsync(It.IsAny<Order>())).ReturnsAsync(orderToReturn);
            _mapperMock.Setup(x => x.Map<OrderResponseDto>(It.IsAny<Order>())).Returns(mappedDto);

            // Act
            var result = await _orderService.CreateOrderAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Order placed successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(employeeId, result.Data.EmployeeID);
            Assert.Equal(chefId, result.Data.ChefID);

            _employeeRepoMock.Verify(x => x.GetEmployeeWithCompanyAsync(employeeId), Times.Once);
            _mealRepoMock.Verify(x => x.GetMealsByIdsAsync(It.Is<List<int>>(ids => ids.SequenceEqual(mealIds))), Times.Once);
            _orderRepoMock.Verify(x => x.CreateOrderAsync(It.IsAny<Order>()), Times.Once);
            _mapperMock.Verify(x => x.Map<OrderResponseDto>(It.IsAny<Order>()), Times.Once);
        }
    }
}