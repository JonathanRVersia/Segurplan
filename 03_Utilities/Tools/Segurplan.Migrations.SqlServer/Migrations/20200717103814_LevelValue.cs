﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class LevelValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LevelValue",
                table: "RiskLevel",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LevelValue",
                table: "RiskLevel");
        }
    }
}
