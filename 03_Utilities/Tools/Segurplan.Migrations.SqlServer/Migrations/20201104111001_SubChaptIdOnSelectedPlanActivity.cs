using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class SubChaptIdOnSelectedPlanActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubChaptId",
                table: "PlanActivityVersion",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubChaptId",
                table: "PlanActivityVersion");
        }
    }
}
