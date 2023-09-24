using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeganLifeDataCenter.Migrations
{
    /// <inheritdoc />
    public partial class InitialReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PercentBreakfast = table.Column<int>(type: "INTEGER", nullable: false),
                    PercentLunch = table.Column<int>(type: "INTEGER", nullable: false),
                    PercentDinner = table.Column<int>(type: "INTEGER", nullable: false),
                    PercentOther = table.Column<int>(type: "INTEGER", nullable: false),
                    GoalKcal = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportModels");
        }
    }
}
