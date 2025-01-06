using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changes_in_func : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysToRenew",
                table: "EndpointCloseToExpiry");

            migrationBuilder.DropColumn(
                name: "Disabled",
                table: "EndpointCloseToExpiry");

            migrationBuilder.DropColumn(
                name: "RenewDate",
                table: "EndpointCloseToExpiry");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysToRenew",
                table: "EndpointCloseToExpiry",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Disabled",
                table: "EndpointCloseToExpiry",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RenewDate",
                table: "EndpointCloseToExpiry",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
