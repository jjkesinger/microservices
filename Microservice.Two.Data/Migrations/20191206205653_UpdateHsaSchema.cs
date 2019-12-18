using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservice.Two.Data.Migrations
{
    public partial class UpdateHsaSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Hsa");

            migrationBuilder.RenameColumn(
                name: "DeductionAmount",
                table: "Hsa",
                newName: "ContributionAmount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContributionAmount",
                table: "Hsa",
                newName: "DeductionAmount");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Hsa",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Hsa",
                keyColumn: "Id",
                keyValue: 1,
                column: "CompanyId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Hsa",
                keyColumn: "Id",
                keyValue: 2,
                column: "CompanyId",
                value: 1);
        }
    }
}
