using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class organizationalStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationStructure",
                table: "SafetyStudyPlanDetails");

            migrationBuilder.AddColumn<string>(
                name: "OrganizationalStructure",
                table: "SafetyStudyPlanDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationalStructure",
                table: "SafetyStudyPlanDetails");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationStructure",
                table: "SafetyStudyPlanDetails",
                nullable: false,
                defaultValue: 0);
        }
    }
}
