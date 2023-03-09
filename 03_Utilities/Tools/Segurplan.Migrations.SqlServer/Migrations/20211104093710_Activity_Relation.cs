using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class Activity_Relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelationsId",
                table: "ActivityVersion",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdRelations = table.Column<int>(nullable: false),
                    IdChapterRelation = table.Column<int>(nullable: true),
                    IdSubchapterRelation = table.Column<int>(nullable: true),
                    IdActivityRelation = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityRelations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityRelations");

            migrationBuilder.DropColumn(
                name: "RelationsId",
                table: "ActivityVersion");
        }
    }
}
