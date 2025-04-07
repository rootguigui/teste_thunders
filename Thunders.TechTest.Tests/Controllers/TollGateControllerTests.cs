using Microsoft.AspNetCore.Mvc;
using Moq;
using Thunders.TechTest.ApiService.Controllers;
using Thunders.TechTest.ApiService.Interfaces.Service;
using Thunders.TechTest.ApiService.Models.Request;
using Xunit;

namespace Thunders.TechTest.Tests.Controllers;

public class TollGateControllerTests
{
    private readonly Mock<ITollGateService> _mockTollGateService;
    private readonly TollGateController _controller;

    public TollGateControllerTests()
    {
        _mockTollGateService = new Mock<ITollGateService>();
        _controller = new TollGateController(_mockTollGateService.Object);
    }

    [Fact]
    public async Task RegisterUsage_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new CreateusageRequest
        {
            TollGateUsage = "TestGate",
            City = "TestCity",
            VehicleType = ApiService.Models.Entities.VehicleType.Car,
            Amount = 10.0m
        };

        _mockTollGateService.Setup(x => x.ProcessUsageAsync(It.IsAny<CreateusageRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RegisterUsage(request);

        // Assert
        Assert.IsType<OkResult>(result);
        _mockTollGateService.Verify(x => x.ProcessUsageAsync(request), Times.Once);
    }

    [Fact]
    public async Task RegisterUsage_NullRequest_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.RegisterUsage(null!);

        // Assert
        Assert.IsType<BadRequestResult>(result);
        _mockTollGateService.Verify(x => x.ProcessUsageAsync(It.IsAny<CreateusageRequest>()), Times.Never);
    }
}