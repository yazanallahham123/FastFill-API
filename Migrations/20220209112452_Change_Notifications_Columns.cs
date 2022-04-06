using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class Change_Notifications_Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReferenceType",
                table: "Notifications",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "ReferenceId",
                table: "Notifications",
                newName: "Time");

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Notifications",
                newName: "ReferenceType");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Notifications",
                newName: "ReferenceId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
