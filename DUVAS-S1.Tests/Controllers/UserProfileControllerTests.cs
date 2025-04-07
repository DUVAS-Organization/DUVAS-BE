using API.Controllers.UserAPI;
using DTO;
using DUVAS;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.IRepository;
using System.Threading.Tasks;
using Xunit;

public class UserProfileControllerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserProfileController _controller;

    public UserProfileControllerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _controller = new UserProfileController(_mockUserRepository.Object, null, null, null, null, null, null, null);
    }

    [Fact]
    public async Task GetUserProfile_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var mockUser = new User(userId, "TestUser", "123456789"); // Sử dụng constructor có tham số

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(mockUser);

        // Act
        var result = await _controller.GetUserProfile(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<User>(okResult.Value);
        Assert.Equal(userId, returnValue.UserId);
        Assert.Equal("TestUser", returnValue.UserName);
    }

    [Fact]
    public async Task GetUserProfile_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 1;
        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.GetUserProfile(userId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var returnValue = Assert.IsType<dynamic>(notFoundResult.Value);
        Assert.Equal("User không tồn tại.", returnValue.Message);
    }

    [Fact]
    public async Task EditProfile_ReturnsOk_WhenUserIsUpdated()
    {
        // Arrange
        var userId = 1;
        var mockUser = new User(userId, "TestUser", "123456789"); // Sử dụng constructor có tham số

        var updateRequest = new EditProfileRequest
        {
            UserName = "UpdatedUser",
            Phone = "987654321"
        };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(mockUser);

        // Act
        var result = await _controller.EditProfile(userId, updateRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Profile updated successfully", okResult.Value);
        Assert.Equal("UpdatedUser", mockUser.UserName);
        Assert.Equal("987654321", mockUser.Phone);
    }

    [Fact]
    public async Task EditProfile_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 1;
        var updateRequest = new EditProfileRequest
        {
            UserName = "UpdatedUser",
            Phone = "987654321"
        };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.EditProfile(userId, updateRequest);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var returnValue = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("User not found", returnValue);
    }
}
