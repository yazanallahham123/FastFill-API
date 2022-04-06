using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class ChangedPaymentTransactionCompany5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_CompanyBranches",
                table: "PaymentTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyBranchId",
                table: "PaymentTransactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_CompanyBranches_CompanyBranchId",
                table: "PaymentTransactions",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_CompanyBranches_CompanyBranchId",
                table: "PaymentTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyBranchId",
                table: "PaymentTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_CompanyBranches",
                table: "PaymentTransactions",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
