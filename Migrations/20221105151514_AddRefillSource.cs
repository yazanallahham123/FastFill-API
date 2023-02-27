using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class AddRefillSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RefillSourceId",
                table: "UserRefillTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RefillSourceId",
                table: "UserCredits",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefillSourceId",
                table: "UserRefillTransactions");

            migrationBuilder.DropColumn(
                name: "RefillSourceId",
                table: "UserCredits");
        }
    }
}
