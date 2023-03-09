using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class AddPreventiveMeasureOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreventiveMeasureOrder",
                table: "RiskAndPreventiveMeasuresMeasures",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreventiveMeasureOrder",
                table: "RiskAndPreventiveMeasuresMeasures");
        }
    }
}
