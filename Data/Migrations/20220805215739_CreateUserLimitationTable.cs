using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fazilat.Data.Migrations
{
    public partial class CreateUserLimitationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLimitation",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    Expiration = table.Column<DateTime>(type: "date", nullable: false, defaultValue: new DateTime(2022, 8, 6, 2, 27, 38, 253, DateTimeKind.Local).AddTicks(6079))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLimitation", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserLimitation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLimitation");
        }
    }
}
