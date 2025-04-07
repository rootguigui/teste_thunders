using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.Models.Entities;
using Thunders.TechTest.ApiService.Repositories;

namespace Thunders.TechTest.Tests.Repository;

public class TollGateRepositoryTests
{
    private readonly Fixture _fixture;
    private readonly DbContextOptions<TollGateDbContext> _options;
    private readonly TollGateDbContext _context;
    private readonly TollGateRepository _repository;

    public TollGateRepositoryTests()
    {
        _fixture = new Fixture();
        _options = new DbContextOptionsBuilder<TollGateDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new TollGateDbContext(_options);
        _repository = new TollGateRepository(_context);
    }

    [Fact]
    public async Task SaveUsageAsync_ShouldAddAndSaveUsage()
    {
        // Arrange
        var usage = _fixture.Create<TollGateUsage>();
        usage.TollGateUsageId = 1;

        // Act
        await _repository.SaveUsageAsync(usage);

        // Assert
        var savedUsage = await _context.TollGateUsages.FindAsync(usage.TollGateUsageId);
        savedUsage.Should().NotBeNull();
        savedUsage.Should().BeEquivalentTo(usage);
    }

    [Fact]
    public async Task GetHourlyValuesByCityAsync_ShouldReturnHourlyValues()
    {
        // Arrange
        var city = _fixture.Create<string>();
        var date = _fixture.Create<DateTime>().Date;
        var usages = _fixture.CreateMany<TollGateUsage>(24).ToList();

        for (int i = 0; i < usages.Count; i++)
        {
            usages[i].TollGateUsageId = i + 1;
            usages[i].City = city;
            usages[i].UsageDateTime = date.AddHours(i);
            usages[i].Amount = _fixture.Create<decimal>();
        }

        await _context.TollGateUsages.AddRangeAsync(usages);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetHourlyValuesByCityAsync(city, date);

        // Assert
        result.Should().HaveCount(24);
        result.Should().BeInAscendingOrder(x => x.Hour);
        result.Should().AllSatisfy(x =>
        {
            x.City.Should().Be(city);
            x.TotalValue.Should().BeGreaterOrEqualTo(0);
        });
    }

    [Fact]
    public async Task GetTopTollGatesByMonthAsync_ShouldReturnTopTollGates()
    {
        // Arrange
        var month = _fixture.Create<int>() % 12 + 1;
        var currentYear = DateTime.Now.Year;
        var year = currentYear + _fixture.Create<int>() % 5;
        var count = _fixture.Create<int>() % 10 + 1;
        var usages = _fixture.CreateMany<TollGateUsage>(count * 2).ToList();

        for (int i = 0; i < usages.Count; i++)
        {
            usages[i].TollGateUsageId = i + 1;
            usages[i].UsageDateTime = new DateTime(year, month, 1).AddDays(i % 28);
            usages[i].Amount = _fixture.Create<decimal>();
        }

        await _context.TollGateUsages.AddRangeAsync(usages);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTopTollGatesByMonthAsync(month, year, count);

        // Assert
        result.Should().HaveCount(count);
        result.Should().BeInDescendingOrder(x => x.TotalRevenue);
        result.Should().AllSatisfy(x =>
        {
            x.Month.Should().Be(month);
            x.Year.Should().Be(year);
            x.TotalRevenue.Should().BeGreaterOrEqualTo(0);
        });
    }

    [Fact]
    public async Task GetVehicleTypeCountByTollGateAsync_ShouldReturnVehicleTypeCount()
    {
        // Arrange
        var tollGate = _fixture.Create<string>();
        var usages = _fixture.CreateMany<TollGateUsage>(30).ToList();

        for (int i = 0; i < usages.Count; i++)
        {
            usages[i].TollGateUsageId = i + 1;
            usages[i].TollGate = tollGate;
            usages[i].VehicleType = (VehicleType)(i % 3);
        }

        await _context.TollGateUsages.AddRangeAsync(usages);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetVehicleTypeCountByTollGateAsync(tollGate);

        // Assert
        result.TollGate.Should().Be(tollGate);
        result.MotorcycleCount.Should().Be(usages.Count(x => x.VehicleType == VehicleType.Motorcycle));
        result.CarCount.Should().Be(usages.Count(x => x.VehicleType == VehicleType.Car));
        result.TruckCount.Should().Be(usages.Count(x => x.VehicleType == VehicleType.Truck));
    }
}