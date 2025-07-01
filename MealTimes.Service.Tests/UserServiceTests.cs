using AutoMapper;
using MealTimes.Core.DTOs;
using MealTimes.Core.Models;
using MealTimes.Core.Repository;
using MealTimes.Core.Service;
using Moq;

namespace MealTimes.Service.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IJwtTokenGenerator> _jwtMock = new();
        private readonly Mock<ICorporateCompanyRepository> _companyRepoMock = new();
        private readonly Mock<IEmployeeRepository> _employeeRepoMock = new();
        private readonly Mock<IHomeChefRepository> _homeChefRepoMock = new();
        private readonly Mock<IDeliveryPersonRepository> _deliveryPersonRepoMock = new();

        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService(
                _userRepoMock.Object,
                _mapperMock.Object,
                _jwtMock.Object,
                _companyRepoMock.Object,
                _employeeRepoMock.Object,
                _homeChefRepoMock.Object,
                _deliveryPersonRepoMock.Object
            );
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "secure123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                UserID = 1,
                Email = email,
                PasswordHash = hashedPassword,
                Role = "Employee"
            };

            var token = "mocked.jwt.token";
            var loginDto = new LoginDto { Email = email, Password = password };

            var userDto = new UserDto
            {
                UserID = user.UserID,
                Email = user.Email,
                Role = user.Role
            };

            _userRepoMock.Setup(x => x.GetUserByEmailAsync(email)).ReturnsAsync(user);
            _jwtMock.Setup(x => x.GenerateToken(user)).Returns(token);
            _mapperMock.Setup(x => x.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Login successful.", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(token, result.Data.Token);
            Assert.Equal(userDto.Email, result.Data.UserDto.Email);

            _userRepoMock.Verify(x => x.GetUserByEmailAsync(email), Times.Once);
            _jwtMock.Verify(x => x.GenerateToken(user), Times.Once);
            _mapperMock.Verify(x => x.Map<UserDto>(user), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldFail_WhenUserNotFound()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "notfound@example.com", Password = "irrelevant" };

            _userRepoMock.Setup(x => x.GetUserByEmailAsync(loginDto.Email)).ReturnsAsync((User)null!);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid credentials", result.Message);
            Assert.Null(result.Data);
            _userRepoMock.Verify(x => x.GetUserByEmailAsync(loginDto.Email), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldFail_WhenPasswordIsIncorrect()
        {
            // Arrange
            var email = "test@example.com";
            var loginDto = new LoginDto { Email = email, Password = "wrongpassword" };

            var user = new User
            {
                UserID = 1,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
                Role = "Employee"
            };

            _userRepoMock.Setup(x => x.GetUserByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid credentials", result.Message);
            Assert.Null(result.Data);

            _userRepoMock.Verify(x => x.GetUserByEmailAsync(email), Times.Once);
            _jwtMock.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
        }
    }
}
