using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class fileSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FileSize",
                table: "SafetyStudyPlanFile",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FileSize",
                table: "DefaultSafetyStudyPlanFile",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "SafetyStudyPlanFile");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "DefaultSafetyStudyPlanFile");
        }
    }
}
