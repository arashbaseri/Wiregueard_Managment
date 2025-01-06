using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFunc_Get_Endpoints_Close_To_Expire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EndpointCloseToExpiry",
                columns: table => new
                {
                    MikrotikInterface = table.Column<string>(type: "text", nullable: false),
                    RenewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DaysToRenew = table.Column<int>(type: "integer", nullable: true),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndpointCloseToExpiry");
        }
    }
}
