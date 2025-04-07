using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thunders.TechTest.ApiService.Models.Entities;

namespace Thunders.TechTest.ApiService.Data.Mappings;

public class TollGateReportMap : IEntityTypeConfiguration<TollGateReport>
{
    public void Configure(EntityTypeBuilder<TollGateReport> builder)
    {
        builder.ToTable("TollGateReports", "public");
        builder.HasKey(t => t.TollGateReportId);
        builder.Property(t => t.TollGateReportId).UseIdentityColumn().HasColumnName("tollgate_report_id").HasColumnType("integer");
        builder.Property(t => t.Content).HasColumnName("content").HasColumnType("text");
        builder.Property(t => t.TollGateReportScheduledId).HasColumnName("tollgate_report_scheduled_id").HasColumnType("integer");
        builder.HasOne(t => t.TollGateReportScheduled).WithMany().HasForeignKey(t => t.TollGateReportScheduledId);
    }
}
