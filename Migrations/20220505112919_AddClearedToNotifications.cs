using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class AddClearedToNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cleared",
                table: "Notifications",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cleared",
                table: "Notifications");
        }
    }
}
