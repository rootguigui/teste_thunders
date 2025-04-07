using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Data.Mappings;
using Thunders.TechTest.ApiService.Models.Entities;

namespace Thunders.TechTest.ApiService.Data;

public class TollGateDbContext : DbContext
{
    public TollGateDbContext(DbContextOptions<TollGateDbContext> options)
        : base(options)
    {
    }

    public DbSet<TollGateUsage> TollGateUsages { get; set; } = null!;
    public DbSet<TollGateReport> TollGateReports { get; set; } = null!;
    public DbSet<TollGateReportScheduled> TollGateReportScheduled { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TollGateUsageMap());
        modelBuilder.ApplyConfiguration(new TollGateReportMap());
        modelBuilder.ApplyConfiguration(new TollGateReportScheduledMap());
    }
}