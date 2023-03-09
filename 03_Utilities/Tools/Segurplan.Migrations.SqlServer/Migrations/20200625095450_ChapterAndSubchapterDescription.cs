using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class ChapterAndSubchapterDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChapterDescription",
                table: "PlanActivityVersion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubChapterDescription",
                table: "PlanActivityVersion",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChapterDescription",
                table: "PlanActivityVersion");

            migrationBuilder.DropColumn(
                name: "SubChapterDescription",
                table: "PlanActivityVersion");
        }
    }
}
