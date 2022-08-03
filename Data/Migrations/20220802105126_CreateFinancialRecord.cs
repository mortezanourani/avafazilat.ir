using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fazilat.Data.Migrations
{
    public partial class CreateFinancialRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialRecord",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    TrackingCode = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    PaymentReceipt = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialRecord_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialRecord_UserId",
                table: "FinancialRecord",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialRecord");
        }
    }
}
