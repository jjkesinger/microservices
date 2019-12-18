using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservice.Two.Data.Migrations
{
    public partial class EmployeeLastPycheckDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPaycheckDate",
                table: "Employee",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPaycheckDate",
                table: "Employee");
        }
    }
}
