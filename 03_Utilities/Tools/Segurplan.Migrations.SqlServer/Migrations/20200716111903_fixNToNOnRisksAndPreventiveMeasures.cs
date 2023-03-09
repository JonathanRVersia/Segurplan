using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class fixNToNOnRisksAndPreventiveMeasures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreventiveMeasure_RisksAndPreventiveMeasures_RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure");

            migrationBuilder.DropIndex(
                name: "IX_PreventiveMeasure_RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure");

            migrationBuilder.DropColumn(
                name: "RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure");

            migrationBuilder.CreateTable(
                name: "RiskAndPreventiveMeasuresMeasures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RisksAndPreventiveMeasuresId = table.Column<int>(nullable: false),
                    PreventiveMeasureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskAndPreventiveMeasuresMeasures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskAndPreventiveMeasuresMeasures_PreventiveMeasure_PreventiveMeasureId",
                        column: x => x.PreventiveMeasureId,
                        principalTable: "PreventiveMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RiskAndPreventiveMeasuresMeasures_RisksAndPreventiveMeasures_RisksAndPreventiveMeasuresId",
                        column: x => x.RisksAndPreventiveMeasuresId,
                        principalTable: "RisksAndPreventiveMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskAndPreventiveMeasuresMeasures_PreventiveMeasureId",
                table: "RiskAndPreventiveMeasuresMeasures",
                column: "PreventiveMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAndPreventiveMeasuresMeasures_RisksAndPreventiveMeasuresId",
                table: "RiskAndPreventiveMeasuresMeasures",
                column: "RisksAndPreventiveMeasuresId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskAndPreventiveMeasuresMeasures");

            migrationBuilder.AddColumn<int>(
                name: "RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMeasure_RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure",
                column: "RisksAndPreventiveMeasuresId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreventiveMeasure_RisksAndPreventiveMeasures_RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure",
                column: "RisksAndPreventiveMeasuresId",
                principalTable: "RisksAndPreventiveMeasures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
