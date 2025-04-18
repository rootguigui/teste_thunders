﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Thunders.TechTest.ApiService.Data;

#nullable disable

namespace Thunders.TechTest.ApiService.Migrations
{
    [DbContext(typeof(TollGateDbContext))]
    [Migration("20250407120324_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Thunders.TechTest.ApiService.Models.Entities.TollGateReport", b =>
                {
                    b.Property<int>("TollGateReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("tollgate_report_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TollGateReportId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<int>("TollGateReportScheduledId")
                        .HasColumnType("integer")
                        .HasColumnName("tollgate_report_scheduled_id");

                    b.HasKey("TollGateReportId");

                    b.HasIndex("TollGateReportScheduledId");

                    b.ToTable("TollGateReports", "public");
                });

            modelBuilder.Entity("Thunders.TechTest.ApiService.Models.Entities.TollGateReportScheduled", b =>
                {
                    b.Property<int>("TollGateReportScheduledId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TollGateReportScheduledId"));

                    b.Property<bool>("IsProcessed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_processed");

                    b.Property<Dictionary<string, object>>("Parameters")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("parameters");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("timestamptz")
                        .HasColumnName("processed_date");

                    b.Property<string>("ReportType")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("report_type");

                    b.Property<DateTime>("ScheduledDate")
                        .HasColumnType("timestamptz")
                        .HasColumnName("scheduled_date");

                    b.HasKey("TollGateReportScheduledId");

                    b.ToTable("TollGateReportScheduled", "public");
                });

            modelBuilder.Entity("Thunders.TechTest.ApiService.Models.Entities.TollGateUsage", b =>
                {
                    b.Property<int>("TollGateUsageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("tollgate_usage_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TollGateUsageId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("city");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("state");

                    b.Property<string>("TollGate")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("tollgate");

                    b.Property<DateTime>("UsageDateTime")
                        .HasColumnType("timestamptz");

                    b.Property<int>("VehicleType")
                        .HasColumnType("integer")
                        .HasColumnName("vehicle_type");

                    b.HasKey("TollGateUsageId");

                    b.ToTable("TollGateUsages", "public");
                });

            modelBuilder.Entity("Thunders.TechTest.ApiService.Models.Entities.TollGateReport", b =>
                {
                    b.HasOne("Thunders.TechTest.ApiService.Models.Entities.TollGateReportScheduled", "TollGateReportScheduled")
                        .WithMany()
                        .HasForeignKey("TollGateReportScheduledId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TollGateReportScheduled");
                });
#pragma warning restore 612, 618
        }
    }
}
