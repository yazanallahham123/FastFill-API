using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class PumpsDatabase3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyPumpId",
                table: "PaymentTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_CompanyPumpId",
                table: "PaymentTransactions",
                column: "CompanyPumpId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_CompanyPumps",
                table: "PaymentTransactions",
                column: "CompanyPumpId",
                principalTable: "CompanyPumps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_CompanyPumps",
                table: "PaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTransactions_CompanyPumpId",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "CompanyPumpId",
                table: "PaymentTransactions");
        }
    }
}
