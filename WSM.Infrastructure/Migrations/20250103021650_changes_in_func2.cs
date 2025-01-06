using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changes_in_func2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllowedAddress",
                table: "EndpointCloseToExpiry",
                type: "varchar",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedAddress",
                table: "EndpointCloseToExpiry");
        }
    }
}
