using API.Controllers;
using API.Service;
using BusinessObject;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Repositories.IRepository;
using Xunit;
using FluentAssertions;
using DUVAS;
using System;

namespace API.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<JwtService> _mockJwtService;
        private readonly AuthController _controller;

        // Mock other services that aren't needed for login tests
        private readonly Mock<EmailService> _mockEmailService;
        private readonly Mock<OtpService> _mockOtpService;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<TokenExchangeService> _mockTokenExchangeService;
        private readonly Mock<TokenDictionaryService> _mockTokenDictionaryService;


        public AuthControllerTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockJwtService = new Mock<JwtService>();

            // Initialize other mocks with default values
            _mockEmailService = new Mock<EmailService>();
            _mockOtpService = new Mock<OtpService>();
            _mockConfig = new Mock<IConfiguration>();
            _mockTokenExchangeService = new Mock<TokenExchangeService>();
            _mockTokenDictionaryService = new Mock<TokenDictionaryService>();

            _controller = new AuthController(
                _mockEmailService.Object,
                _mockOtpService.Object,
                _mockUserRepo.Object,
                _mockJwtService.Object,
                _mockConfig.Object,
                _mockTokenExchangeService.Object,
                _mockTokenDictionaryService.Object);
        }

        private User CreateTestUser(string username, string password)
        {
            return new User(
                gmail: $"{username}@example.com",
                userName: username,
                name: "Test User",
                password: BCrypt.Net.BCrypt.HashPassword(password),
                address: "123 Test St",
                sex: "Male",
                profilePicture: "test.jpg",
                money: 1000,
                roleUser: 1
            )
            {
                UserId = 1,
                Phone = "0123456789"
            };
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var testUser = CreateTestUser("testuser", "password");
            var loginDto = new LoginDTO
            {
                Username = "testuser",
                Password = "password"
            };

            _mockUserRepo.Setup(x => x.GetUserByUsernameAsync(loginDto.Username))
                .ReturnsAsync(testUser);

            _mockJwtService.Setup(x => x.GenerateToken(It.IsAny<User>()))
                .Returns("mock-jwt-token");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(new { Message = "mock-jwt-token" });
        }

        [Fact]
        public async Task Login_WithValidEmail_ReturnsToken()
        {
            // Arrange
            var testUser = CreateTestUser("testuser", "password");
            var loginDto = new LoginDTO
            {
                Username = "testuser@example.com", // Using email as username
                Password = "password"
            };

            _mockUserRepo.Setup(x => x.GetUserByGmailOrPhoneAsync(loginDto.Username))
                .ReturnsAsync(testUser);

            _mockJwtService.Setup(x => x.GenerateToken(It.IsAny<User>()))
                .Returns("mock-jwt-token");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(new { Message = "mock-jwt-token" });
        }

        [Fact]
        public async Task Login_WithInvalidUsername_ReturnsBadRequest()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Username = "nonexistent",
                Password = "password"
            };

            _mockUserRepo.Setup(x => x.GetUserByGmailOrPhoneAsync(loginDto.Username))
                .ReturnsAsync((User)null);

            _mockUserRepo.Setup(x => x.GetUserByUsernameAsync(loginDto.Username))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().BeEquivalentTo(new { Message = "Tài khoản hoặc mật khẩu sai" });
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsBadRequest()
        {
            // Arrange
            var testUser = CreateTestUser("testuser", "correctpassword");
            var loginDto = new LoginDTO
            {
                Username = "testuser",
                Password = "wrongpassword"
            };

            _mockUserRepo.Setup(x => x.GetUserByUsernameAsync(loginDto.Username))
                .ReturnsAsync(testUser);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().BeEquivalentTo(new { Message = "Tài khoản hoặc mật khẩu sai" });
        }
    }
}