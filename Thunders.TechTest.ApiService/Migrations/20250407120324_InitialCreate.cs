using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Thunders.TechTest.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "TollGateReportScheduled",
                schema: "public",
                columns: table => new
                {
                    TollGateReportScheduledId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    report_type = table.Column<string>(type: "varchar(255)", nullable: false),
                    scheduled_date = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    parameters = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false),
                    is_processed = table.Column<bool>(type: "boolean", nullable: false),
                    processed_date = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TollGateReportScheduled", x => x.TollGateReportScheduledId);
                });

            migrationBuilder.CreateTable(
                name: "TollGateUsages",
                schema: "public",
                columns: table => new
                {
                    tollgate_usage_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsageDateTime = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    tollgate = table.Column<string>(type: "varchar(255)", nullable: false),
                    city = table.Column<string>(type: "varchar(255)", nullable: false),
                    state = table.Column<string>(type: "varchar(255)", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    vehicle_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TollGateUsages", x => x.tollgate_usage_id);
                });

            migrationBuilder.CreateTable(
                name: "TollGateReports",
                schema: "public",
                columns: table => new
                {
                    tollgate_report_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tollgate_report_scheduled_id = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TollGateReports", x => x.tollgate_report_id);
                    table.ForeignKey(
                        name: "FK_TollGateReports_TollGateReportScheduled_tollgate_report_sch~",
                        column: x => x.tollgate_report_scheduled_id,
                        principalSchema: "public",
                        principalTable: "TollGateReportScheduled",
                        principalColumn: "TollGateReportScheduledId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TollGateReports_tollgate_report_scheduled_id",
                schema: "public",
                table: "TollGateReports",
                column: "tollgate_report_scheduled_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TollGateReports",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TollGateUsages",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TollGateReportScheduled",
                schema: "public");
        }
    }
}
