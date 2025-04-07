using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.Models.Entities;
using Thunders.TechTest.ApiService.Repositories;

namespace Thunders.TechTest.Tests.Repository;

public class TollGateReportScheduledRepositoryTests
{
    private readonly Fixture _fixture;
    private readonly DbContextOptions<TollGateDbContext> _options;
    private readonly TollGateDbContext _context;
    private readonly TollGateReportScheduledRepository _repository;

    public TollGateReportScheduledRepositoryTests()
    {
        _fixture = new Fixture();
        _options = new DbContextOptionsBuilder<TollGateDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new TollGateDbContext(_options);
        _repository = new TollGateReportScheduledRepository(_context);
    }

    [Fact]
    public async Task CreateReportScheduledAsync_ShouldAddAndSaveReportScheduled()
    {
        // Arrange
        var reportScheduled = _fixture.Create<TollGateReportScheduled>();
        reportScheduled.TollGateReportScheduledId = 1; // Definindo um ID fixo para garantir que o teste funcione

        // Act
        var result = await _repository.CreateReportScheduledAsync(reportScheduled);

        // Assert
        result.Should().BeEquivalentTo(reportScheduled);
        var savedReport = await _context.TollGateReportScheduled.FindAsync(reportScheduled.TollGateReportScheduledId);
        savedReport.Should().NotBeNull();
        savedReport.Should().BeEquivalentTo(reportScheduled);
    }

    [Fact]
    public async Task GetReportScheduledAsync_ShouldReturnAllReports()
    {
        // Arrange
        var reports = _fixture.CreateMany<TollGateReportScheduled>(3).ToList();
        for (int i = 0; i < reports.Count; i++)
        {
            reports[i].TollGateReportScheduledId = i + 1; // Definindo IDs sequenciais
        }
        await _context.TollGateReportScheduled.AddRangeAsync(reports);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetReportScheduledAsync();

        // Assert
        result.Should().BeEquivalentTo(reports);
    }

    [Fact]
    public async Task UpdateProgressAsync_WhenReportExists_ShouldUpdateAndSave()
    {
        // Arrange
        var report = _fixture.Create<TollGateReportScheduled>();
        report.TollGateReportScheduledId = 1; // Definindo um ID fixo para garantir que o teste funcione
        report.IsProcessed = false;
        report.ProcessedDate = null;

        await _context.TollGateReportScheduled.AddAsync(report);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.UpdateProgressAsync(report.TollGateReportScheduledId);

        // Assert
        result.Should().BeEquivalentTo(report);
        result.IsProcessed.Should().BeTrue();
        result.ProcessedDate.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateProgressAsync_WhenReportDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var reportId = _fixture.Create<int>();

        // Act & Assert
        await _repository.Invoking(x => x.UpdateProgressAsync(reportId))
            .Should().ThrowAsync<Exception>()
            .WithMessage("Report scheduled not found");
    }
}