using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class AddBudgetTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdBudget",
                table: "SafetyStudyPlan",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArticleFamily",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Family = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleFamily", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleFamily_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleFamily_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Budget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Total = table.Column<decimal>(nullable: false),
                    ApplicabePercentage = table.Column<int>(nullable: false),
                    StudyBudget = table.Column<decimal>(nullable: false),
                    Difference = table.Column<decimal>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Budget_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Budget_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Percentage = table.Column<int>(nullable: false),
                    TimeOfWork = table.Column<decimal>(nullable: false),
                    MinimumUnit = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    AmortizationTime = table.Column<decimal>(nullable: false),
                    IdArticleFamily = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Article_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Article_ArticleFamily_IdArticleFamily",
                        column: x => x.IdArticleFamily,
                        principalTable: "ArticleFamily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Article_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTaskDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdArticle = table.Column<int>(nullable: false),
                    IdTasks = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTaskDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTaskDetail_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleTaskDetail_Article_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleTaskDetail_Tasks_IdTasks",
                        column: x => x.IdTasks,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleTaskDetail_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuantityUnits = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    IdArticle = table.Column<int>(nullable: false),
                    IdBudget = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetDetail_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetDetail_Article_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetDetail_Budget_IdBudget",
                        column: x => x.IdBudget,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetDetail_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdBudget",
                table: "SafetyStudyPlan",
                column: "IdBudget");

            migrationBuilder.CreateIndex(
                name: "IX_Article_CreatedBy",
                table: "Article",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Article_IdArticleFamily",
                table: "Article",
                column: "IdArticleFamily");

            migrationBuilder.CreateIndex(
                name: "IX_Article_ModifiedBy",
                table: "Article",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleFamily_CreatedBy",
                table: "ArticleFamily",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleFamily_ModifiedBy",
                table: "ArticleFamily",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTaskDetail_CreatedBy",
                table: "ArticleTaskDetail",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTaskDetail_IdArticle",
                table: "ArticleTaskDetail",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTaskDetail_IdTasks",
                table: "ArticleTaskDetail",
                column: "IdTasks");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTaskDetail_ModifiedBy",
                table: "ArticleTaskDetail",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Budget_CreatedBy",
                table: "Budget",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Budget_ModifiedBy",
                table: "Budget",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetail_CreatedBy",
                table: "BudgetDetail",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetail_IdArticle",
                table: "BudgetDetail",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetail_IdBudget",
                table: "BudgetDetail",
                column: "IdBudget");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetail_ModifiedBy",
                table: "BudgetDetail",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedBy",
                table: "Tasks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ModifiedBy",
                table: "Tasks",
                column: "ModifiedBy");

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

            migrationBuilder.DropTable(
                name: "ArticleTaskDetail");

            migrationBuilder.DropTable(
                name: "BudgetDetail");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Budget");

            migrationBuilder.DropTable(
                name: "ArticleFamily");

            migrationBuilder.DropIndex(
                name: "IX_SafetyStudyPlan_IdBudget",
                table: "SafetyStudyPlan");

            migrationBuilder.DropColumn(
                name: "IdBudget",
                table: "SafetyStudyPlan");
        }
    }
}
