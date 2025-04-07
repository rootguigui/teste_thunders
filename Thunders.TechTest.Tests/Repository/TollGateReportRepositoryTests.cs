using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.Models.Entities;
using Thunders.TechTest.ApiService.Repositories;

namespace Thunders.TechTest.Tests.Repository;

public class TollGateReportRepositoryTests
{
    private readonly Fixture _fixture;
    private readonly DbContextOptions<TollGateDbContext> _options;
    private readonly TollGateDbContext _context;
    private readonly TollGateReportRepository _repository;

    public TollGateReportRepositoryTests()
    {
        _fixture = new Fixture();
        _options = new DbContextOptionsBuilder<TollGateDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new TollGateDbContext(_options);
        _repository = new TollGateReportRepository(_context);
    }

    [Fact]
    public async Task CreateReportAsync_ShouldAddAndSaveReport()
    {
        // Arrange
        var report = _fixture.Create<TollGateReport>();
        report.TollGateReportId = 1; // Definindo um ID fixo para garantir que o teste funcione

        // Act
        var result = await _repository.CreateReportAsync(report);

        // Assert
        result.Should().BeEquivalentTo(report);
        var savedReport = await _context.TollGateReports.FindAsync(report.TollGateReportId);
        savedReport.Should().NotBeNull();
        savedReport.Should().BeEquivalentTo(report);
    }

    [Fact]
    public async Task GetReportAsync_WhenReportExists_ShouldReturnReport()
    {
        // Arrange
        var report = _fixture.Create<TollGateReport>();

        await _context.TollGateReports.AddAsync(report);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetReportAsync(report.TollGateReportScheduledId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(report);
    }

    [Fact]
    public async Task GetReportAsync_WhenReportDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var reportId = _fixture.Create<int>();

        // Act
        var result = await _repository.GetReportAsync(reportId);

        // Assert
        result.Should().BeNull();
    }
}