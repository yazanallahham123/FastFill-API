using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class AddUserIdAndDateToErrorsLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ErrorLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ErrorLogs",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ErrorLogs");
        }
    }
}
