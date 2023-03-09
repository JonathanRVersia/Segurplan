using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class PlanPlane : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SafetyStudyPlanPlane",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdSafetyStudyPlan = table.Column<int>(nullable: false),
                    IdPlane = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStudyPlanPlane", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanPlane_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanPlane_Plane_IdPlane",
                        column: x => x.IdPlane,
                        principalTable: "Plane",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanPlane_SafetyStudyPlan_IdSafetyStudyPlan",
                        column: x => x.IdSafetyStudyPlan,
                        principalTable: "SafetyStudyPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanPlane_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlane_CreatedBy",
                table: "SafetyStudyPlanPlane",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlane_IdPlane",
                table: "SafetyStudyPlanPlane",
                column: "IdPlane");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlane_IdSafetyStudyPlan",
                table: "SafetyStudyPlanPlane",
                column: "IdSafetyStudyPlan");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlane_ModifiedBy",
                table: "SafetyStudyPlanPlane",
                column: "ModifiedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SafetyStudyPlanPlane");
        }
    }
}
