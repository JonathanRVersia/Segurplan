using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class chapter_SubchapterPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChapterPosition",
                table: "PlanActivityVersion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubChapterPosition",
                table: "PlanActivityVersion",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChapterPosition",
                table: "PlanActivityVersion");

            migrationBuilder.DropColumn(
                name: "SubChapterPosition",
                table: "PlanActivityVersion");
        }
    }
}
