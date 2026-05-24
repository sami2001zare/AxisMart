using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxisMart.Infra.Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class OtpFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PenaltyTime",
                table: "OTPs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "PenaltyTime",
                table: "OTPs",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
