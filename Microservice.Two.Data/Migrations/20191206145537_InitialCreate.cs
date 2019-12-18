using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservice.Two.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hsa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    DeductionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hsa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Ssn = table.Column<string>(nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paycheck",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    PayDate = table.Column<DateTime>(nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paycheck", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paycheck_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deduction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaycheckId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deduction_Paycheck_PaycheckId",
                        column: x => x.PaycheckId,
                        principalTable: "Paycheck",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "JHL Enterprises" },
                    { 2, "HCSS" },
                    { 3, "Hello Fresh" },
                    { 4, "Walmart" },
                    { 5, "Transafe" }
                });

            migrationBuilder.InsertData(
                table: "Hsa",
                columns: new[] { "Id", "CompanyId", "DeductionAmount", "EmployeeId" },
                values: new object[,]
                {
                    { 1, 1, 20m, 4 },
                    { 2, 1, 40m, 2 }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "CompanyId", "FirstName", "LastName", "Salary", "Ssn", "Status" },
                values: new object[,]
                {
                    { 2, 1, "John", "Kesinger", 40000m, "123333232", 0 },
                    { 3, 1, "James", "Doe", 80000m, "878676767", 0 },
                    { 4, 1, "Amanda", "Know", 20000m, "989887878", 1 },
                    { 1, 5, "Steve", "Smith", 35000m, "373637363", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_PaycheckId",
                table: "Deduction",
                column: "PaycheckId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CompanyId",
                table: "Employee",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Paycheck_EmployeeId",
                table: "Paycheck",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deduction");

            migrationBuilder.DropTable(
                name: "Hsa");

            migrationBuilder.DropTable(
                name: "Paycheck");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
