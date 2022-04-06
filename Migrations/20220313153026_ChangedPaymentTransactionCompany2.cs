using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class ChangedPaymentTransactionCompany2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "PaymentTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_CompanyId",
                table: "PaymentTransactions",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_Companies",
                table: "PaymentTransactions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Companies",
                table: "PaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTransactions_CompanyId",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "PaymentTransactions");
        }
    }
}
