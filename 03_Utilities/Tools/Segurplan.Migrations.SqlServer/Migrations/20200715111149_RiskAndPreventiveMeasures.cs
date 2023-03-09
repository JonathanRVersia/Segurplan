using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class RiskAndPreventiveMeasures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Probability",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Probability", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Risk",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskLevel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seriousness",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seriousness", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RisksAndPreventiveMeasures",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChapterId = table.Column<int>(nullable: false),
                    SubChapterId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    RiskId = table.Column<int>(nullable: false),
                    ProbabilityId = table.Column<int>(nullable: false),
                    SeriousnessId = table.Column<int>(nullable: false),
                    RiskLevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RisksAndPreventiveMeasures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_Chapter_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_Probability_ProbabilityId",
                        column: x => x.ProbabilityId,
                        principalTable: "Probability",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_RiskLevel_RiskLevelId",
                        column: x => x.RiskLevelId,
                        principalTable: "RiskLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_Seriousness_SeriousnessId",
                        column: x => x.SeriousnessId,
                        principalTable: "Seriousness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_SubChapter_SubChapterId",
                        column: x => x.SubChapterId,
                        principalTable: "SubChapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMeasure_RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure",
                column: "RisksAndPreventiveMeasuresId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_ActivityId",
                table: "RisksAndPreventiveMeasures",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_ChapterId",
                table: "RisksAndPreventiveMeasures",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_ProbabilityId",
                table: "RisksAndPreventiveMeasures",
                column: "ProbabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_RiskId",
                table: "RisksAndPreventiveMeasures",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_RiskLevelId",
                table: "RisksAndPreventiveMeasures",
                column: "RiskLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_SeriousnessId",
                table: "RisksAndPreventiveMeasures",
                column: "SeriousnessId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_SubChapterId",
                table: "RisksAndPreventiveMeasures",
                column: "SubChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreventiveMeasure_RisksAndPreventiveMeasures_RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure",
                column: "RisksAndPreventiveMeasuresId",
                principalTable: "RisksAndPreventiveMeasures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreventiveMeasure_RisksAndPreventiveMeasures_RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure");

            migrationBuilder.DropTable(
                name: "RisksAndPreventiveMeasures");

            migrationBuilder.DropTable(
                name: "Probability");

            migrationBuilder.DropTable(
                name: "Risk");

            migrationBuilder.DropTable(
                name: "RiskLevel");

            migrationBuilder.DropTable(
                name: "Seriousness");

            migrationBuilder.DropIndex(
                name: "IX_PreventiveMeasure_RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure");

            migrationBuilder.DropColumn(
                name: "RisksAndPreventiveMeasuresId",
                table: "PreventiveMeasure");
        }
    }
}
