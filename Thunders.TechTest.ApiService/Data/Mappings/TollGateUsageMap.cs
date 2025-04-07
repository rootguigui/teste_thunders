using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thunders.TechTest.ApiService.Models.Entities;

namespace Thunders.TechTest.ApiService.Data.Mappings;

public class TollGateUsageMap : IEntityTypeConfiguration<TollGateUsage>
{
    public void Configure(EntityTypeBuilder<TollGateUsage> builder)
    {
        builder.ToTable("TollGateUsages", "public");
        builder.HasKey(t => t.TollGateUsageId);
        builder.Property(t => t.TollGateUsageId).UseIdentityColumn().HasColumnName("tollgate_usage_id").HasColumnType("integer");
        builder.Property(t => t.Amount).HasColumnType("decimal(18,2)");
        builder.Property(t => t.UsageDateTime).HasColumnType("timestamptz");
        builder.Property(t => t.TollGate).HasColumnName("tollgate").HasColumnType("varchar(255)");
        builder.Property(t => t.City).HasColumnName("city").HasColumnType("varchar(255)");
        builder.Property(t => t.State).HasColumnName("state").HasColumnType("varchar(255)");
        builder.Property(t => t.VehicleType).HasColumnName("vehicle_type").HasColumnType("integer");
    }
}

