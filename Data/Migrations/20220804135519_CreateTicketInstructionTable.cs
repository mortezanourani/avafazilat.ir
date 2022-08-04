using Microsoft.EntityFrameworkCore.Migrations;

namespace Fazilat.Data.Migrations
{
    public partial class CreateTicketInstructionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketInstruction",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInstruction", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TicketInstruction",
                columns: new[] { "Id", "Content", "Title" },
                values: new object[] { "6821b5a5-befe-4aa3-92ac-0be54c817857", "محتوای دستورالعمل درخواست مشاوره", "عنوان اصلی صفحه" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketInstruction");
        }
    }
}
