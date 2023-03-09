using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class EmergencyPlanType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmergencyPlan",
                table: "SafetyStudyPlanDetails",
                newName: "EmergencyPlanDescription");

            migrationBuilder.AlterColumn<bool>(
                name: "AffectedServices",
                table: "SafetyStudyPlanDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AffectedServicesDescription",
                table: "SafetyStudyPlanDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdEmergencyPlanType",
                table: "SafetyStudyPlanDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmergencyPlanType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Caption = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyPlanType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanDetails_IdEmergencyPlanType",
                table: "SafetyStudyPlanDetails",
                column: "IdEmergencyPlanType");

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyStudyPlanDetails_EmergencyPlanType_IdEmergencyPlanType",
                table: "SafetyStudyPlanDetails",
                column: "IdEmergencyPlanType",
                principalTable: "EmergencyPlanType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafetyStudyPlanDetails_EmergencyPlanType_IdEmergencyPlanType",
                table: "SafetyStudyPlanDetails");

            migrationBuilder.DropTable(
                name: "EmergencyPlanType");

            migrationBuilder.DropIndex(
                name: "IX_SafetyStudyPlanDetails_IdEmergencyPlanType",
                table: "SafetyStudyPlanDetails");

            migrationBuilder.DropColumn(
                name: "AffectedServicesDescription",
                table: "SafetyStudyPlanDetails");

            migrationBuilder.DropColumn(
                name: "IdEmergencyPlanType",
                table: "SafetyStudyPlanDetails");

            migrationBuilder.RenameColumn(
                name: "EmergencyPlanDescription",
                table: "SafetyStudyPlanDetails",
                newName: "EmergencyPlan");

            migrationBuilder.AlterColumn<string>(
                name: "AffectedServices",
                table: "SafetyStudyPlanDetails",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
