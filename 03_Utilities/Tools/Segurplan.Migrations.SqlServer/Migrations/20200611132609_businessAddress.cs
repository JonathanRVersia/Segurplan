using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class businessAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delegation_BusinessAddress_IdBusinessAddress",
                table: "Delegation");

            migrationBuilder.DropIndex(
                name: "IX_Delegation_IdBusinessAddress",
                table: "Delegation");

            migrationBuilder.DropColumn(
                name: "IdBusinessAddress",
                table: "Delegation");

            migrationBuilder.AddColumn<int>(
                name: "IdBusinessAddress",
                table: "SafetyStudyPlan",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdBusinessAddress",
                table: "SafetyStudyPlan",
                column: "IdBusinessAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyStudyPlan_BusinessAddress_IdBusinessAddress",
                table: "SafetyStudyPlan",
                column: "IdBusinessAddress",
                principalTable: "BusinessAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafetyStudyPlan_BusinessAddress_IdBusinessAddress",
                table: "SafetyStudyPlan");

            migrationBuilder.DropIndex(
                name: "IX_SafetyStudyPlan_IdBusinessAddress",
                table: "SafetyStudyPlan");

            migrationBuilder.DropColumn(
                name: "IdBusinessAddress",
                table: "SafetyStudyPlan");

            migrationBuilder.AddColumn<int>(
                name: "IdBusinessAddress",
                table: "Delegation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Delegation_IdBusinessAddress",
                table: "Delegation",
                column: "IdBusinessAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_Delegation_BusinessAddress_IdBusinessAddress",
                table: "Delegation",
                column: "IdBusinessAddress",
                principalTable: "BusinessAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
