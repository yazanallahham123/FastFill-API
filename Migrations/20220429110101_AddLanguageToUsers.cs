using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class AddLanguageToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Users",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Users");
        }
    }
}
