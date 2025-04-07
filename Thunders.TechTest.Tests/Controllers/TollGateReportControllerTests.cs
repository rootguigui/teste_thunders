using Microsoft.AspNetCore.Mvc;
using Moq;
using Thunders.TechTest.ApiService.Controllers;
using Thunders.TechTest.ApiService.Interfaces.Service;
using Thunders.TechTest.ApiService.Models.Request;
using Thunders.TechTest.ApiService.Models.Response;
using Thunders.TechTest.ApiService.Models.Entities;
using Xunit;

namespace Thunders.TechTest.Tests.Controllers;

public class TollGateReportControllerTests
{
    private readonly Mock<IReportService> _mockReportService;
    private readonly TollGateReportController _controller;

    public TollGateReportControllerTests()
    {
        _mockReportService = new Mock<IReportService>();
        _controller = new TollGateReportController(_mockReportService.Object);
    }

    [Fact]
    public async Task ScheduleReport_ValidRequest_ReturnsAcceptedResult()
    {
        // Arrange
        var request = new ScheduleReportRequest
        {
            ReportType = "TestReport",
            ScheduledDate = DateTime.Now,
            Parameters = new Dictionary<string, object>()
        };

        _mockReportService.Setup(x => x.ScheduleReportAsync(
            It.IsAny<string>(),
            It.IsAny<DateTime>(),
            It.IsAny<Dictionary<string, object>>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ScheduleReport(request);

        // Assert
        Assert.IsType<AcceptedResult>(result);
        _mockReportService.Verify(x => x.ScheduleReportAsync(
            request.ReportType,
            request.ScheduledDate,
            request.Parameters), Times.Once);
    }

    [Fact]
    public async Task GetReportScheduled_ValidRequest_ReturnsOkResultWithData()
    {
        // Arrange
        var expectedReports = new List<TollGateReportScheduled>();
        _mockReportService.Setup(x => x.GetReportScheduledAsync())
            .ReturnsAsync(expectedReports);

        // Act
        var result = await _controller.GetReportScheduled();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<TollGateReportScheduled>>(okResult.Value);
        Assert.Same(expectedReports, returnValue);
    }

    [Fact]
    public async Task GetReportScheduled_WithId_ValidRequest_ReturnsOkResultWithData()
    {
        // Arrange
        var reportId = 1;
        var expectedReport = new TollGateReport();
        _mockReportService.Setup(x => x.GetReportAsync(reportId))
            .ReturnsAsync(expectedReport);

        // Act
        var result = await _controller.GetReportScheduled(reportId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<TollGateReport>(okResult.Value);
        Assert.Same(expectedReport, returnValue);
    }

    [Fact]
    public async Task ScheduleReport_ServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var request = new ScheduleReportRequest();
        _mockReportService.Setup(x => x.ScheduleReportAsync(
            It.IsAny<string>(),
            It.IsAny<DateTime>(),
            It.IsAny<Dictionary<string, object>>()))
            .ThrowsAsync(new Exception("Erro ao agendar relatório"));

        // Act
        var result = await _controller.ScheduleReport(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var objectResult = result as BadRequestObjectResult;
        Assert.Equal(400, objectResult?.StatusCode);
    }
}