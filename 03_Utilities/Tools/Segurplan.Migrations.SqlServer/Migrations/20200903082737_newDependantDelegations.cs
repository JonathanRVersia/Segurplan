using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class newDependantDelegations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessAddressId",
                table: "Delegation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Delegation_BusinessAddressId",
                table: "Delegation",
                column: "BusinessAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delegation_BusinessAddress_BusinessAddressId",
                table: "Delegation",
                column: "BusinessAddressId",
                principalTable: "BusinessAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delegation_BusinessAddress_BusinessAddressId",
                table: "Delegation");

            migrationBuilder.DropIndex(
                name: "IX_Delegation_BusinessAddressId",
                table: "Delegation");

            migrationBuilder.DropColumn(
                name: "BusinessAddressId",
                table: "Delegation");
        }
    }
}
