using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EndpointUsages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MikrotikEndpointId = table.Column<Guid>(type: "uuid", nullable: true),
                    MikrotikServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BytesIn = table.Column<long>(type: "bigint", nullable: false),
                    BytesOut = table.Column<long>(type: "bigint", nullable: false),
                    PacketsIn = table.Column<long>(type: "bigint", nullable: true),
                    PacketsOut = table.Column<long>(type: "bigint", nullable: true),
                    BytesTotal = table.Column<long>(type: "bigint", nullable: true),
                    PacketsTotal = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndpointUsages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MikrotikCHRs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    WinboxPort = table.Column<int>(type: "integer", nullable: true),
                    WWWPort = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MikrotikCHRs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndpointUsages");

            migrationBuilder.DropTable(
                name: "MikrotikCHRs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
