using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class NoNullableAuditColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AffiliatedCompany_Users_ModifiedBy",
                table: "AffiliatedCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAddress_UsersUpdate",
                table: "BusinessAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapter_Users_ModifiedBy",
                table: "Chapter");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Users_ModifiedBy",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Delegation_UsersUpdate",
                table: "Delegation");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanReview_UsersUpdate",
                table: "PlanReview");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanType_UsersUpdate",
                table: "PlanType");

            migrationBuilder.DropForeignKey(
                name: "FK_PreventiveMeasure_Users_ModifiedBy",
                table: "PreventiveMeasure");

            migrationBuilder.DropForeignKey(
                name: "FK_SubChapterVersion_Users_ModifiedBy",
                table: "SubChapterVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_Template_UsersUpdate",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "ChapterVersionInfo");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "ChapterVersionInfo");

            migrationBuilder.RenameColumn(
                name: "ModificationUser",
                table: "ChapterVersionInfo",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "CreationUser",
                table: "ChapterVersionInfo",
                newName: "CreatedBy");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Template",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Template",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "SubChapterVersion",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "SubChapterVersion",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "PreventiveMeasure",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "PreventiveMeasure",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "PlanType",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "PlanType",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "PlanReview",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "PlanReview",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Delegation",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Delegation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Customer",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Customer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ChapterVersionInfo",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "ChapterVersionInfo",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Chapter",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Chapter",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "BusinessAddress",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "BusinessAddress",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "AffiliatedCompany",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "AffiliatedCompany",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AffiliatedCompany_Users_ModifiedBy",
                table: "AffiliatedCompany",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAddress_UsersUpdate",
                table: "BusinessAddress",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chapter_Users_ModifiedBy",
                table: "Chapter",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Users_ModifiedBy",
                table: "Customer",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Delegation_UsersUpdate",
                table: "Delegation",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanReview_UsersUpdate",
                table: "PlanReview",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanType_UsersUpdate",
                table: "PlanType",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreventiveMeasure_Users_ModifiedBy",
                table: "PreventiveMeasure",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubChapterVersion_Users_ModifiedBy",
                table: "SubChapterVersion",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_UsersUpdate",
                table: "Template",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AffiliatedCompany_Users_ModifiedBy",
                table: "AffiliatedCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAddress_UsersUpdate",
                table: "BusinessAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapter_Users_ModifiedBy",
                table: "Chapter");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Users_ModifiedBy",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Delegation_UsersUpdate",
                table: "Delegation");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanReview_UsersUpdate",
                table: "PlanReview");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanType_UsersUpdate",
                table: "PlanType");

            migrationBuilder.DropForeignKey(
                name: "FK_PreventiveMeasure_Users_ModifiedBy",
                table: "PreventiveMeasure");

            migrationBuilder.DropForeignKey(
                name: "FK_SubChapterVersion_Users_ModifiedBy",
                table: "SubChapterVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_Template_UsersUpdate",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ChapterVersionInfo");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "ChapterVersionInfo");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "ChapterVersionInfo",
                newName: "ModificationUser");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ChapterVersionInfo",
                newName: "CreationUser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Template",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Template",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "SubChapterVersion",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "SubChapterVersion",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "PreventiveMeasure",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "PreventiveMeasure",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "PlanType",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "PlanType",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "PlanReview",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "PlanReview",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Delegation",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Delegation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Customer",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Customer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "ChapterVersionInfo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "ChapterVersionInfo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Chapter",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Chapter",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "BusinessAddress",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "BusinessAddress",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "AffiliatedCompany",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "AffiliatedCompany",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AffiliatedCompany_Users_ModifiedBy",
                table: "AffiliatedCompany",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAddress_UsersUpdate",
                table: "BusinessAddress",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chapter_Users_ModifiedBy",
                table: "Chapter",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Users_ModifiedBy",
                table: "Customer",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Delegation_UsersUpdate",
                table: "Delegation",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanReview_UsersUpdate",
                table: "PlanReview",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanType_UsersUpdate",
                table: "PlanType",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreventiveMeasure_Users_ModifiedBy",
                table: "PreventiveMeasure",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubChapterVersion_Users_ModifiedBy",
                table: "SubChapterVersion",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_UsersUpdate",
                table: "Template",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
