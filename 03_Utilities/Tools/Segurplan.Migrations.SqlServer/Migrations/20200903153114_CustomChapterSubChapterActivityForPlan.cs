using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class CustomChapterSubChapterActivityForPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanActivityVersion_ActivityVersion_IdActivityVersion",
                table: "PlanActivityVersion");

            migrationBuilder.AlterColumn<int>(
                name: "IdActivityVersion",
                table: "PlanActivityVersion",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CustomActivityId",
                table: "PlanActivityVersion",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomChapter",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomChapter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomSubchapter",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CustomChapterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomSubchapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomSubchapter_CustomChapter_CustomChapterId",
                        column: x => x.CustomChapterId,
                        principalTable: "CustomChapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomActivity",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CustomSubchapterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomActivity_CustomSubchapter_CustomSubchapterId",
                        column: x => x.CustomSubchapterId,
                        principalTable: "CustomSubchapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanActivityVersion_CustomActivityId",
                table: "PlanActivityVersion",
                column: "CustomActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomActivity_CustomSubchapterId",
                table: "CustomActivity",
                column: "CustomSubchapterId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomSubchapter_CustomChapterId",
                table: "CustomSubchapter",
                column: "CustomChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanActivityVersion_CustomActivity_CustomActivityId",
                table: "PlanActivityVersion",
                column: "CustomActivityId",
                principalTable: "CustomActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanActivityVersion_ActivityVersion_IdActivityVersion",
                table: "PlanActivityVersion",
                column: "IdActivityVersion",
                principalTable: "ActivityVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanActivityVersion_CustomActivity_CustomActivityId",
                table: "PlanActivityVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanActivityVersion_ActivityVersion_IdActivityVersion",
                table: "PlanActivityVersion");

            migrationBuilder.DropTable(
                name: "CustomActivity");

            migrationBuilder.DropTable(
                name: "CustomSubchapter");

            migrationBuilder.DropTable(
                name: "CustomChapter");

            migrationBuilder.DropIndex(
                name: "IX_PlanActivityVersion_CustomActivityId",
                table: "PlanActivityVersion");

            migrationBuilder.DropColumn(
                name: "CustomActivityId",
                table: "PlanActivityVersion");

            migrationBuilder.AlterColumn<int>(
                name: "IdActivityVersion",
                table: "PlanActivityVersion",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanActivityVersion_ActivityVersion_IdActivityVersion",
                table: "PlanActivityVersion",
                column: "IdActivityVersion",
                principalTable: "ActivityVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
