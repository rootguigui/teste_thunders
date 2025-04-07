using AutoFixture;
using FluentAssertions;
using Moq;
using Thunders.TechTest.ApiService.Models.Entities;
using Thunders.TechTest.ApiService.Models.Request;
using Thunders.TechTest.ApiService.Services;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.Tests.Services;

public class TollGateServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IMessageSender> _messageSenderMock;
    private readonly TollGateService _tollGateService;

    public TollGateServiceTests()
    {
        _fixture = new Fixture();
        _messageSenderMock = new Mock<IMessageSender>();
        _tollGateService = new TollGateService(_messageSenderMock.Object);
    }

    [Fact]
    public async Task ProcessUsageAsync_ShouldSendMessage()
    {
        // Arrange
        var usage = _fixture.Create<CreateusageRequest>();

        // Act
        await _tollGateService.ProcessUsageAsync(usage);

        // Assert
        _messageSenderMock.Verify(x => x.SendLocal(It.Is<TollGateUsage>(m =>
            m.TollGate == usage.TollGateUsage &&
            m.City == usage.City &&
            m.VehicleType == usage.VehicleType &&
            m.Amount == usage.Amount)), Times.Once);
    }
}