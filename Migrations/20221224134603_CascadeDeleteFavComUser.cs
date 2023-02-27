using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class CascadeDeleteFavComUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCompanies_Users",
                table: "FavoriteCompanies");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCompanies_Users",
                table: "FavoriteCompanies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCompanies_Users",
                table: "FavoriteCompanies");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCompanies_Users",
                table: "FavoriteCompanies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
