using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class UpdateSafetyStudyPlanDetailsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafetyStudyPlan_Budget_IdBudget",
                table: "SafetyStudyPlan");

            migrationBuilder.DropColumn(
                name: "WorkerAverage",
                table: "SafetyStudyPlanDetails");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Budget");

            migrationBuilder.RenameColumn(
                name: "MaximumWorkers",
                table: "SafetyStudyPlanDetails",
                newName: "WorkersNumber");

            migrationBuilder.AlterColumn<int>(
                name: "IdBudget",
                table: "SafetyStudyPlan",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyStudyPlan_Budget_IdBudget",
                table: "SafetyStudyPlan",
                column: "IdBudget",
                principalTable: "Budget",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafetyStudyPlan_Budget_IdBudget",
                table: "SafetyStudyPlan");

            migrationBuilder.RenameColumn(
                name: "WorkersNumber",
                table: "SafetyStudyPlanDetails",
                newName: "MaximumWorkers");

            migrationBuilder.AddColumn<double>(
                name: "WorkerAverage",
                table: "SafetyStudyPlanDetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "IdBudget",
                table: "SafetyStudyPlan",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Budget",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyStudyPlan_Budget_IdBudget",
                table: "SafetyStudyPlan",
                column: "IdBudget",
                principalTable: "Budget",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
