using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class FilestreamOnPlanPlanes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafetyStudyPlanPlane_Plane_IdPlane",
                table: "SafetyStudyPlanPlane");

            migrationBuilder.AddColumn<Guid>(
                name: "Data_Id",
                table: "SafetyStudyPlanPlaneFile",
                type: "UNIQUEIDENTIFIER ROWGUIDCOL UNIQUE",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SafetyStudyPlanPlaneFile",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Record_Id",
                table: "SafetyStudyPlanPlaneFile",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdPlane",
                table: "SafetyStudyPlanPlane",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SafetyStudyPlanPlane",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FamilyName",
                table: "SafetyStudyPlanPlane",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlaneFile_Record_Id",
                table: "SafetyStudyPlanPlaneFile",
                column: "Record_Id",
                unique: true,
                filter: "[Record_Id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyStudyPlanPlane_Plane_IdPlane",
                table: "SafetyStudyPlanPlane",
                column: "IdPlane",
                principalTable: "Plane",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafetyStudyPlanPlane_Plane_IdPlane",
                table: "SafetyStudyPlanPlane");

            migrationBuilder.DropIndex(
                name: "IX_SafetyStudyPlanPlaneFile_Record_Id",
                table: "SafetyStudyPlanPlaneFile");

            migrationBuilder.DropColumn(
                name: "Data_Id",
                table: "SafetyStudyPlanPlaneFile");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SafetyStudyPlanPlaneFile");

            migrationBuilder.DropColumn(
                name: "Record_Id",
                table: "SafetyStudyPlanPlaneFile");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SafetyStudyPlanPlane");

            migrationBuilder.DropColumn(
                name: "FamilyName",
                table: "SafetyStudyPlanPlane");

            migrationBuilder.AlterColumn<int>(
                name: "IdPlane",
                table: "SafetyStudyPlanPlane",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyStudyPlanPlane_Plane_IdPlane",
                table: "SafetyStudyPlanPlane",
                column: "IdPlane",
                principalTable: "Plane",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
