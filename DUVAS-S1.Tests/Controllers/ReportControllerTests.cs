using API.Controllers.UserAPI;
using DTO;
using DUVAS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.IRepository;
using System.Security.Claims;
using Xunit;

public class ReportControllerTests
{
    private ReportController CreateControllerWithUserClaims(string? userIdClaim)
    {
        var reportRepoMock = new Mock<IReportRepository>();

        var controller = new ReportController(reportRepoMock.Object);

        var userClaims = new List<Claim>();
        if (userIdClaim != null)
            userClaims.Add(new Claim("UserId", userIdClaim));

        var identity = new ClaimsIdentity(userClaims, "mock");
        var principal = new ClaimsPrincipal(identity);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        return controller;
    }

    [Fact]
    public async Task AddReport_WithValidUserId_ReturnsOk()
    {
        // Arrange
        var mockRepo = new Mock<IReportRepository>();
        mockRepo.Setup(r => r.SaveReportAsync(It.IsAny<Report>())).Returns(Task.CompletedTask);

        var controller = new ReportController(mockRepo.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim("UserId", "123")
                }))
            }
        };

        var dto = new AddReportDto { ReportContent = "Test", Image = "image.jpg" };

        // Act
        var result = await controller.AddReport(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        mockRepo.Verify(r => r.SaveReportAsync(It.IsAny<Report>()), Times.Once);
    }

    [Fact]
    public async Task AddReport_WithoutUserIdClaim_ReturnsBadRequest()
    {
        var controller = CreateControllerWithUserClaims(null);
        var dto = new AddReportDto { ReportContent = "Test", Image = "image.jpg" };

        var result = await controller.AddReport(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("UserId claim not found.", badRequest.Value);
    }

    [Fact]
    public async Task AddReport_WithInvalidUserId_ReturnsBadRequest()
    {
        var controller = CreateControllerWithUserClaims("abc");
        var dto = new AddReportDto { ReportContent = "Test", Image = "image.jpg" };

        var result = await controller.AddReport(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid UserId.", badRequest.Value);
    }
}
