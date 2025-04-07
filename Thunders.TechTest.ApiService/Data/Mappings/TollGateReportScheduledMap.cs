using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thunders.TechTest.ApiService.Models.Entities;

namespace Thunders.TechTest.ApiService.Data.Mappings;

public class TollGateReportScheduledMap : IEntityTypeConfiguration<TollGateReportScheduled>
{
    public void Configure(EntityTypeBuilder<TollGateReportScheduled> builder)
    {
        builder.ToTable("TollGateReportScheduled", "public");
        builder.HasKey(t => t.TollGateReportScheduledId);
        builder.Property(t => t.ReportType).HasColumnName("report_type").HasColumnType("varchar(255)");
        builder.Property(t => t.ScheduledDate).HasColumnName("scheduled_date").HasColumnType("timestamptz");
        builder.Property(t => t.Parameters).HasColumnName("parameters").HasColumnType("jsonb");
        builder.Property(t => t.IsProcessed).HasColumnName("is_processed").HasColumnType("boolean");
        builder.Property(t => t.ProcessedDate).HasColumnName("processed_date").HasColumnType("timestamptz");
    }
}
