using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class RiskLevelBySeriousnessAndProbability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskLevelBySeriousnessAndProbabilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SeriousnessId = table.Column<int>(nullable: false),
                    ProbabilityId = table.Column<int>(nullable: false),
                    RiskLevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskLevelBySeriousnessAndProbabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskLevelBySeriousnessAndProbabilities_Probability_ProbabilityId",
                        column: x => x.ProbabilityId,
                        principalTable: "Probability",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskLevelBySeriousnessAndProbabilities_RiskLevel_RiskLevelId",
                        column: x => x.RiskLevelId,
                        principalTable: "RiskLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskLevelBySeriousnessAndProbabilities_Seriousness_SeriousnessId",
                        column: x => x.SeriousnessId,
                        principalTable: "Seriousness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskLevelBySeriousnessAndProbabilities_ProbabilityId",
                table: "RiskLevelBySeriousnessAndProbabilities",
                column: "ProbabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskLevelBySeriousnessAndProbabilities_RiskLevelId",
                table: "RiskLevelBySeriousnessAndProbabilities",
                column: "RiskLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskLevelBySeriousnessAndProbabilities_SeriousnessId",
                table: "RiskLevelBySeriousnessAndProbabilities",
                column: "SeriousnessId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskLevelBySeriousnessAndProbabilities");
        }
    }
}
