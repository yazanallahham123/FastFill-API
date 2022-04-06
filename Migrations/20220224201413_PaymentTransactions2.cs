using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class PaymentTransactions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransaction_CompanyBranches_CompanyBranchId",
                table: "PaymentTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransaction_Users_UserId",
                table: "PaymentTransaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentTransaction",
                table: "PaymentTransaction");

            migrationBuilder.RenameTable(
                name: "PaymentTransaction",
                newName: "PaymentTransactions");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransaction_UserId",
                table: "PaymentTransactions",
                newName: "IX_PaymentTransactions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransaction_CompanyBranchId",
                table: "PaymentTransactions",
                newName: "IX_PaymentTransactions_CompanyBranchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentTransactions",
                table: "PaymentTransactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_CompanyBranches",
                table: "PaymentTransactions",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_Users",
                table: "PaymentTransactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_CompanyBranches",
                table: "PaymentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Users",
                table: "PaymentTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentTransactions",
                table: "PaymentTransactions");

            migrationBuilder.RenameTable(
                name: "PaymentTransactions",
                newName: "PaymentTransaction");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransactions_UserId",
                table: "PaymentTransaction",
                newName: "IX_PaymentTransaction_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransactions_CompanyBranchId",
                table: "PaymentTransaction",
                newName: "IX_PaymentTransaction_CompanyBranchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentTransaction",
                table: "PaymentTransaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransaction_CompanyBranches_CompanyBranchId",
                table: "PaymentTransaction",
                column: "CompanyBranchId",
                principalTable: "CompanyBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransaction_Users_UserId",
                table: "PaymentTransaction",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
