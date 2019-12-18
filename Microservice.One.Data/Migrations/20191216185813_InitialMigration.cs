using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservice.One.Data.Migrations
{
    public partial class InitialMigration : Migration
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
                    EmployeeId = table.Column<int>(nullable: false),
                    Balance = table.Column<decimal>(nullable: false),
                    ContributionAmount = table.Column<decimal>(nullable: false)
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
                columns: new[] { "Id", "Balance", "ContributionAmount", "EmployeeId" },
                values: new object[,]
                {
                    { 1, 0m, 20m, 4 },
                    { 2, 0m, 40m, 2 }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "CompanyId", "FirstName", "LastName", "Ssn", "Status" },
                values: new object[,]
                {
                    { 2, 1, "John", "Kesinger", "123333232", 0 },
                    { 3, 1, "James", "Doe", "878676767", 0 },
                    { 4, 1, "Amanda", "Know", "989887878", 1 },
                    { 1, 5, "Steve", "Smith", "373637363", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CompanyId",
                table: "Employee",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Hsa");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
