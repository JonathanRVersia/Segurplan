using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class DeleteChapterVersionInfoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapter_ChapterVersionInfo_IdVersionInfo",
                table: "Chapter");

            migrationBuilder.DropForeignKey(
                name: "FK_ChapterVersion_ChapterVersionInfo_IdVersionInfo",
                table: "ChapterVersion");

            migrationBuilder.DropTable(
                name: "ChapterVersionInfo");

            migrationBuilder.DropIndex(
                name: "IX_ChapterVersion_IdVersionInfo",
                table: "ChapterVersion");

            migrationBuilder.DropIndex(
                name: "IX_Chapter_IdVersionInfo",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "IdVersionInfo",
                table: "Chapter");

            migrationBuilder.RenameColumn(
                name: "IdVersionInfo",
                table: "ChapterVersion",
                newName: "VersionNumber");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ChapterVersion",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ChapterVersion",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ChapterVersion");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ChapterVersion");

            migrationBuilder.RenameColumn(
                name: "VersionNumber",
                table: "ChapterVersion",
                newName: "IdVersionInfo");

            migrationBuilder.AddColumn<int>(
                name: "IdVersionInfo",
                table: "Chapter",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChapterVersionInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterVersionInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChapterVersion_IdVersionInfo",
                table: "ChapterVersion",
                column: "IdVersionInfo");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_IdVersionInfo",
                table: "Chapter",
                column: "IdVersionInfo");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapter_ChapterVersionInfo_IdVersionInfo",
                table: "Chapter",
                column: "IdVersionInfo",
                principalTable: "ChapterVersionInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterVersion_ChapterVersionInfo_IdVersionInfo",
                table: "ChapterVersion",
                column: "IdVersionInfo",
                principalTable: "ChapterVersionInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
