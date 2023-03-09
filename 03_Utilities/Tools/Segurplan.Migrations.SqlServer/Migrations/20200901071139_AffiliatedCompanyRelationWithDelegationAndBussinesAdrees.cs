using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class AffiliatedCompanyRelationWithDelegationAndBussinesAdrees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AffiliatedCompanyId",
                table: "Delegation",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AffiliatedCompanyId",
                table: "BusinessAddress",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Delegation_AffiliatedCompanyId",
                table: "Delegation",
                column: "AffiliatedCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessAddress_AffiliatedCompanyId",
                table: "BusinessAddress",
                column: "AffiliatedCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAddress_AffiliatedCompany_AffiliatedCompanyId",
                table: "BusinessAddress",
                column: "AffiliatedCompanyId",
                principalTable: "AffiliatedCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Delegation_AffiliatedCompany_AffiliatedCompanyId",
                table: "Delegation",
                column: "AffiliatedCompanyId",
                principalTable: "AffiliatedCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAddress_AffiliatedCompany_AffiliatedCompanyId",
                table: "BusinessAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_Delegation_AffiliatedCompany_AffiliatedCompanyId",
                table: "Delegation");

            migrationBuilder.DropIndex(
                name: "IX_Delegation_AffiliatedCompanyId",
                table: "Delegation");

            migrationBuilder.DropIndex(
                name: "IX_BusinessAddress_AffiliatedCompanyId",
                table: "BusinessAddress");

            migrationBuilder.DropColumn(
                name: "AffiliatedCompanyId",
                table: "Delegation");

            migrationBuilder.DropColumn(
                name: "AffiliatedCompanyId",
                table: "BusinessAddress");
        }
    }
}
