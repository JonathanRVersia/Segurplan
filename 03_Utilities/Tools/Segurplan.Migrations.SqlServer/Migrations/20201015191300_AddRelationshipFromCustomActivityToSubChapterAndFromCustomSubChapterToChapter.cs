using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class AddRelationshipFromCustomActivityToSubChapterAndFromCustomSubChapterToChapter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChapterId",
                table: "CustomSubchapter",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubChapterId",
                table: "CustomActivity",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomSubchapter_ChapterId",
                table: "CustomSubchapter",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomActivity_SubChapterId",
                table: "CustomActivity",
                column: "SubChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomActivity_SubChapter_SubChapterId",
                table: "CustomActivity",
                column: "SubChapterId",
                principalTable: "SubChapter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomSubchapter_Chapter_ChapterId",
                table: "CustomSubchapter",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomActivity_SubChapter_SubChapterId",
                table: "CustomActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomSubchapter_Chapter_ChapterId",
                table: "CustomSubchapter");

            migrationBuilder.DropIndex(
                name: "IX_CustomSubchapter_ChapterId",
                table: "CustomSubchapter");

            migrationBuilder.DropIndex(
                name: "IX_CustomActivity_SubChapterId",
                table: "CustomActivity");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "CustomSubchapter");

            migrationBuilder.DropColumn(
                name: "SubChapterId",
                table: "CustomActivity");
        }
    }
}
