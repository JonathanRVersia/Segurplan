using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class PlaneFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SafetyStudyPlanPlaneFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(nullable: true),
                    Data = table.Column<byte[]>(nullable: true),
                    SafetyStudyPlanPlaneId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStudyPlanPlaneFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanPlaneFile_SafetyStudyPlanPlane_SafetyStudyPlanPlaneId",
                        column: x => x.SafetyStudyPlanPlaneId,
                        principalTable: "SafetyStudyPlanPlane",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlaneFile_SafetyStudyPlanPlaneId",
                table: "SafetyStudyPlanPlaneFile",
                column: "SafetyStudyPlanPlaneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SafetyStudyPlanPlaneFile");
        }
    }
}
