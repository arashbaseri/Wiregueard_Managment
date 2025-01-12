using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class gernerateQRcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountRemaining",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConfigEndPoint",
                table: "MikrotikCHRs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ConfigEndPointPort",
                table: "MikrotikCHRs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConfigPublicKey",
                table: "MikrotikCHRs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountRemaining",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConfigEndPoint",
                table: "MikrotikCHRs");

            migrationBuilder.DropColumn(
                name: "ConfigEndPointPort",
                table: "MikrotikCHRs");

            migrationBuilder.DropColumn(
                name: "ConfigPublicKey",
                table: "MikrotikCHRs");
        }
    }
}
