using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MikrotikEndpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MikrotikEndpoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MikrotikServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    MikrotikInterface = table.Column<string>(type: "text", nullable: false),
                    AllowedAddress = table.Column<string>(type: "varchar", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    PublicKey = table.Column<string>(type: "varchar", nullable: true),
                    PrivateKey = table.Column<string>(type: "varchar", nullable: true),
                    RenewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DaysToRenew = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MikrotikEndpoints", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MikrotikEndpoints");
        }
    }
}
