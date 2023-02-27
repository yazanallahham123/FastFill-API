using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FastFill_API.Migrations
{
    public partial class PumpsDatabase2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyPumps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPumps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPumps_Companies",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAgentPumps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    PumpId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAgentPumps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyAgentPumps_CompanyPumps_PumpId",
                        column: x => x.PumpId,
                        principalTable: "CompanyPumps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyAgentPumps_Users_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPumpsState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyAgentPumpId = table.Column<int>(type: "int", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    OpenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPumpsState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPumpsState_CompanyPumps",
                        column: x => x.CompanyAgentPumpId,
                        principalTable: "CompanyAgentPumps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAgentPumps_AgentId",
                table: "CompanyAgentPumps",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAgentPumps_PumpId",
                table: "CompanyAgentPumps",
                column: "PumpId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPumps_CompanyId",
                table: "CompanyPumps",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPumpsState_CompanyAgentPumpId",
                table: "CompanyPumpsState",
                column: "CompanyAgentPumpId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyPumpsState");

            migrationBuilder.DropTable(
                name: "CompanyAgentPumps");

            migrationBuilder.DropTable(
                name: "CompanyPumps");
        }
    }
}
