using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class AddedUserCredits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyUser",
                table: "CompanyUser");

            migrationBuilder.RenameTable(
                name: "CompanyUser",
                newName: "CompanyUsers");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyUser_UserId",
                table: "CompanyUsers",
                newName: "IX_CompanyUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyUser_CompanyId",
                table: "CompanyUsers",
                newName: "IX_CompanyUsers_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers");

            migrationBuilder.RenameTable(
                name: "CompanyUsers",
                newName: "CompanyUser");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyUsers_UserId",
                table: "CompanyUser",
                newName: "IX_CompanyUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyUsers_CompanyId",
                table: "CompanyUser",
                newName: "IX_CompanyUser_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyUser",
                table: "CompanyUser",
                column: "Id");
        }
    }
}
