using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text.Json;
using Thunders.TechTest.ApiService.Interfaces.Repository;
using Thunders.TechTest.ApiService.Models.Entities;
using Thunders.TechTest.ApiService.Models.Request;
using Thunders.TechTest.ApiService.Models.Response;
using Thunders.TechTest.ApiService.Services;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.Tests.Services;

public class ReportServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ITollGateRepository> _tollGateRepositoryMock;
    private readonly Mock<IMessageSender> _messageSenderMock;
    private readonly Mock<ITollGateReportRepository> _tollGateReportRepositoryMock;
    private readonly Mock<ITollGateReportScheduledRepository> _tollGateReportScheduledRepositoryMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly ReportService _reportService;

    public ReportServiceTests()
    {
        _fixture = new Fixture();
        _tollGateRepositoryMock = new Mock<ITollGateRepository>();
        _messageSenderMock = new Mock<IMessageSender>();
        _tollGateReportRepositoryMock = new Mock<ITollGateReportRepository>();
        _tollGateReportScheduledRepositoryMock = new Mock<ITollGateReportScheduledRepository>();
        _cacheMock = new Mock<IDistributedCache>();

        _reportService = new ReportService(
            _tollGateRepositoryMock.Object,
            _messageSenderMock.Object,
            _tollGateReportRepositoryMock.Object,
            _tollGateReportScheduledRepositoryMock.Object,
            _cacheMock.Object
        );
    }

    [Fact]
    public async Task GenerateHourlyValueReportAsync_ShouldReturnHourlyValues()
    {
        // Arrange
        var city = _fixture.Create<string>();
        var date = _fixture.Create<DateTime>();
        var expectedReports = _fixture.CreateMany<HourlyValueReport>().ToList();

        _tollGateRepositoryMock
            .Setup(x => x.GetHourlyValuesByCityAsync(city, date))
            .ReturnsAsync(expectedReports);

        // Act
        var result = await _reportService.GenerateHourlyValueReportAsync(city, date);

        // Assert
        result.Should().BeEquivalentTo(expectedReports);
        _tollGateRepositoryMock.Verify(x => x.GetHourlyValuesByCityAsync(city, date), Times.Once);
    }

    [Fact]
    public async Task GenerateTopTollGatesReportAsync_ShouldReturnTopTollGates()
    {
        // Arrange
        var month = _fixture.Create<int>();
        var year = _fixture.Create<int>();
        var count = _fixture.Create<int>();
        var expectedReports = _fixture.CreateMany<TopTollGatesReport>().ToList();

        _tollGateRepositoryMock
            .Setup(x => x.GetTopTollGatesByMonthAsync(month, year, count))
            .ReturnsAsync(expectedReports);

        // Act
        var result = await _reportService.GenerateTopTollGatesReportAsync(month, year, count);

        // Assert
        result.Should().BeEquivalentTo(expectedReports);
        _tollGateRepositoryMock.Verify(x => x.GetTopTollGatesByMonthAsync(month, year, count), Times.Once);
    }

    [Fact]
    public async Task GenerateVehicleTypeCountReportAsync_ShouldReturnVehicleTypeCount()
    {
        // Arrange
        var tollGate = _fixture.Create<string>();
        var expectedReport = _fixture.Create<VehicleTypeCountReport>();

        _tollGateRepositoryMock
            .Setup(x => x.GetVehicleTypeCountByTollGateAsync(tollGate))
            .ReturnsAsync(expectedReport);

        // Act
        var result = await _reportService.GenerateVehicleTypeCountReportAsync(tollGate);

        // Assert
        result.Should().BeEquivalentTo(expectedReport);
        _tollGateRepositoryMock.Verify(x => x.GetVehicleTypeCountByTollGateAsync(tollGate), Times.Once);
    }

    [Fact]
    public async Task GetReportAsync_ShouldReturnReport()
    {
        // Arrange
        var reportId = _fixture.Create<int>();
        var expectedReport = _fixture.Create<TollGateReport>();

        _tollGateReportRepositoryMock
            .Setup(x => x.GetReportAsync(reportId))
            .ReturnsAsync(expectedReport);

        // Act
        var result = await _reportService.GetReportAsync(reportId);

        // Assert
        result.Should().BeEquivalentTo(expectedReport);
        _tollGateReportRepositoryMock.Verify(x => x.GetReportAsync(reportId), Times.Once);
    }

    [Fact]
    public async Task GetReportScheduledAsync_WhenCacheIsEmpty_ShouldReturnFromRepository()
    {
        // Arrange
        var expectedReports = _fixture.CreateMany<TollGateReportScheduled>().ToList();
        var cacheValue = (byte[]?)null;

        _cacheMock
            .Setup(x => x.GetAsync("scheduled-reports", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cacheValue);

        _tollGateReportScheduledRepositoryMock
            .Setup(x => x.GetReportScheduledAsync())
            .ReturnsAsync(expectedReports);

        // Act
        var result = await _reportService.GetReportScheduledAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedReports);
        _cacheMock.Verify(x => x.GetAsync("scheduled-reports", It.IsAny<CancellationToken>()), Times.Once);
        _tollGateReportScheduledRepositoryMock.Verify(x => x.GetReportScheduledAsync(), Times.Once);
    }

    [Fact]
    public async Task GetReportScheduledAsync_WhenCacheHasValue_ShouldReturnFromCache()
    {
        // Arrange
        var expectedReports = _fixture.CreateMany<TollGateReportScheduled>().ToList();
        var cacheValue = JsonSerializer.Serialize(expectedReports);
        var cacheBytes = System.Text.Encoding.UTF8.GetBytes(cacheValue);

        _cacheMock
            .Setup(x => x.GetAsync("scheduled-reports", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cacheBytes);

        // Act
        var result = await _reportService.GetReportScheduledAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedReports);
        _cacheMock.Verify(x => x.GetAsync("scheduled-reports", It.IsAny<CancellationToken>()), Times.Once);
        _tollGateReportScheduledRepositoryMock.Verify(x => x.GetReportScheduledAsync(), Times.Never);
    }

    [Fact]
    public async Task ScheduleReportAsync_ShouldCreateAndSendReport()
    {
        // Arrange
        var reportType = _fixture.Create<string>();
        var scheduledDate = _fixture.Create<DateTime>();
        var parameters = _fixture.Create<Dictionary<string, object>>();
        var expectedReportScheduled = _fixture.Create<TollGateReportScheduled>();

        _tollGateReportScheduledRepositoryMock
            .Setup(x => x.CreateReportScheduledAsync(It.IsAny<TollGateReportScheduled>()))
            .ReturnsAsync(expectedReportScheduled);

        _cacheMock
            .Setup(x => x.GetAsync("scheduled-reports", It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        // Act
        await _reportService.ScheduleReportAsync(reportType, scheduledDate, parameters);

        // Assert
        _tollGateReportScheduledRepositoryMock.Verify(x => x.CreateReportScheduledAsync(It.IsAny<TollGateReportScheduled>()), Times.Once);
        _messageSenderMock.Verify(x => x.SendLocal(It.IsAny<ReportRequest>()), Times.Once);
    }
}