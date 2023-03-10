using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.DataAccessLayer.Migrations
{
    public partial class MigracionInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdRelations = table.Column<int>(nullable: false),
                    IdChapterRelation = table.Column<int>(nullable: true),
                    IdSubchapterRelation = table.Column<int>(nullable: true),
                    IdActivityRelation = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityRelations", x => x.Id);
                });

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
                name: "DefaultSafetyStudyPlanFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdPlanFileType = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileData = table.Column<byte[]>(nullable: true),
                    FileSize = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultSafetyStudyPlanFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyPlanType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Caption = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyPlanType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StateId = table.Column<long>(nullable: false),
                    StateName = table.Column<string>(nullable: true),
                    InvocationData = table.Column<string>(nullable: true),
                    Arguments = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ExpireAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanDetailsDefaultValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PropName = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanDetailsDefaultValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanFileType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanFileType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Probability",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Probability", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Risk",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskLevel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<string>(nullable: true),
                    TrafficLightsColour = table.Column<string>(nullable: true),
                    LevelValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seriousness",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seriousness", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CompleteName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RiskLevelBySeriousnessAndProbabilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SeriousnessId = table.Column<int>(nullable: false),
                    ProbabilityId = table.Column<int>(nullable: false),
                    RiskLevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskLevelBySeriousnessAndProbabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskLevelBySeriousnessAndProbabilities_Probability_ProbabilityId",
                        column: x => x.ProbabilityId,
                        principalTable: "Probability",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RiskLevelBySeriousnessAndProbabilities_RiskLevel_RiskLevelId",
                        column: x => x.RiskLevelId,
                        principalTable: "RiskLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RiskLevelBySeriousnessAndProbabilities_Seriousness_SeriousnessId",
                        column: x => x.SeriousnessId,
                        principalTable: "Seriousness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AffiliatedCompany",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffiliatedCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffiliatedCompany_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AffiliatedCompany_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ArticleFamily",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Family = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleFamily", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleFamily_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ArticleFamily_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Budget",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicabePercentage = table.Column<int>(nullable: false),
                    StudyBudget = table.Column<decimal>(nullable: false),
                    Difference = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Budget_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Budget_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BusinessAddress",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessAddress_UsersCreate",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessAddress_UsersUpdate",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    WordDescription = table.Column<string>(nullable: true),
                    DefaultSelectedChapter = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapter_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Chapter_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Customer_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GeneralActivity",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralActivity_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GeneralActivity_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlaneFamily",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Family = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaneFamily", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaneFamily_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PlaneFamily_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlanType",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanType_UsersCreate",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanType_UsersUpdate",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PreventiveMeasure",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreventiveMeasure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreventiveMeasure_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PreventiveMeasure_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    TemplateType = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(unicode: false, nullable: true),
                    FilePath = table.Column<string>(unicode: false, nullable: true),
                    File_data = table.Column<byte[]>(nullable: true),
                    FileSize = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Template_UsersCreate",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Template_UsersUpdate",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Percentage = table.Column<int>(nullable: false),
                    TimeOfWork = table.Column<decimal>(nullable: false),
                    MinimumUnit = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    AmortizationTime = table.Column<decimal>(nullable: false),
                    IdArticleFamily = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Article_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Article_ArticleFamily_IdArticleFamily",
                        column: x => x.IdArticleFamily,
                        principalTable: "ArticleFamily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Article_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Delegation",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    BusinessAddressId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delegation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delegation_BusinessAddress_BusinessAddressId",
                        column: x => x.BusinessAddressId,
                        principalTable: "BusinessAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Delegation_UsersCreate",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Delegation_UsersUpdate",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChapterVersion",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdChapter = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    WordDescription = table.Column<string>(nullable: true),
                    WorkDetails = table.Column<string>(nullable: true),
                    WorkOrganization = table.Column<string>(nullable: true),
                    RiskEvaluation = table.Column<string>(nullable: true),
                    MachineTool = table.Column<string>(nullable: true),
                    AssociatedDetails = table.Column<string>(nullable: true),
                    SupportFacilities = table.Column<string>(nullable: true),
                    IdReviewer = table.Column<int>(nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdApprover = table.Column<int>(nullable: true),
                    ApprovementDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterVersion_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ChapterVersion_Users_IdApprover",
                        column: x => x.IdApprover,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChapterVersion_Chapter_IdChapter",
                        column: x => x.IdChapter,
                        principalTable: "Chapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ChapterVersion_Users_IdReviewer",
                        column: x => x.IdReviewer,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChapterVersion_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
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
                    CustomChapterId = table.Column<int>(nullable: true),
                    ChapterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomSubchapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomSubchapter_Chapter_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomSubchapter_CustomChapter_CustomChapterId",
                        column: x => x.CustomChapterId,
                        principalTable: "CustomChapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubChapter",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdChapter = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubChapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubChapter_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SubChapter_Chapter_IdChapter",
                        column: x => x.IdChapter,
                        principalTable: "Chapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SubChapter_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Plane",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FamilyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    IsAvailable = table.Column<bool>(nullable: false, defaultValue: true),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plane", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plane_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Plane_PlaneFamily_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "PlaneFamily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Plane_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTaskDetail",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdArticle = table.Column<int>(nullable: false),
                    IdTasks = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTaskDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTaskDetail_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ArticleTaskDetail_Article_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ArticleTaskDetail_Tasks_IdTasks",
                        column: x => x.IdTasks,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ArticleTaskDetail_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BudgetDetail",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuantityUnits = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    IdArticle = table.Column<int>(nullable: false),
                    IdBudget = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetDetail_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BudgetDetail_Article_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BudgetDetail_Budget_IdBudget",
                        column: x => x.IdBudget,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BudgetDetail_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStudyPlan",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdDelegation = table.Column<int>(nullable: false),
                    IdBudget = table.Column<int>(nullable: false),
                    IdBusinessAddress = table.Column<int>(nullable: false),
                    IdAffiliatedCompany = table.Column<int>(nullable: false),
                    ProjectName = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    IdCustomer = table.Column<int>(nullable: false),
                    PlanCustomerDescription = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    IdGeneralActivity = table.Column<int>(nullable: false),
                    IdPlanType = table.Column<int>(nullable: false),
                    IdTemplate = table.Column<int>(nullable: false),
                    ApproverName = table.Column<string>(nullable: true),
                    IdReviewer = table.Column<int>(nullable: false),
                    CreatorName = table.Column<string>(nullable: true),
                    IdCreatorProfile = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStudyPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_UsersCreate",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_AffiliatedCompany_IdAffiliatedCompany",
                        column: x => x.IdAffiliatedCompany,
                        principalTable: "AffiliatedCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_Budget_IdBudget",
                        column: x => x.IdBudget,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_BusinessAddress_IdBusinessAddress",
                        column: x => x.IdBusinessAddress,
                        principalTable: "BusinessAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_Profile_IdCreatorProfile",
                        column: x => x.IdCreatorProfile,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_Customer_IdCustomer",
                        column: x => x.IdCustomer,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_Delegation_IdDelegation",
                        column: x => x.IdDelegation,
                        principalTable: "Delegation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_GeneralActivity_IdGeneralActivity",
                        column: x => x.IdGeneralActivity,
                        principalTable: "GeneralActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_PlanType",
                        column: x => x.IdPlanType,
                        principalTable: "PlanType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_Users_IdReviewer",
                        column: x => x.IdReviewer,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_Template",
                        column: x => x.IdTemplate,
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_UsersUpdate",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserChapterVersion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ChapterVersionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChapterVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChapterVersion_ChapterVersion_ChapterVersionId",
                        column: x => x.ChapterVersionId,
                        principalTable: "ChapterVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserChapterVersion_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubChapterId = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Description = table.Column<string>(unicode: false, nullable: false),
                    WordDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_UsersCreate",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activity_UsersUpdate",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Activity_SubChapter",
                        column: x => x.SubChapterId,
                        principalTable: "SubChapter",
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
                    CustomSubchapterId = table.Column<int>(nullable: true),
                    SubChapterId = table.Column<int>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_CustomActivity_SubChapter_SubChapterId",
                        column: x => x.SubChapterId,
                        principalTable: "SubChapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubChapterVersion",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdSubChapter = table.Column<int>(nullable: false),
                    IdChapterVersion = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    WorkDetails = table.Column<string>(nullable: true),
                    WorkOrganization = table.Column<string>(nullable: true),
                    RiskEvaluation = table.Column<string>(nullable: true),
                    MachineTool = table.Column<string>(nullable: true),
                    AssociatedDetails = table.Column<string>(nullable: true),
                    SupportFacilities = table.Column<string>(nullable: true),
                    IdReviewer = table.Column<int>(nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdApprover = table.Column<int>(nullable: true),
                    ApprovementDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubChapterVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubChapterVersion_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SubChapterVersion_Users_IdApprover",
                        column: x => x.IdApprover,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubChapterVersion_ChapterVersion_IdChapterVersion",
                        column: x => x.IdChapterVersion,
                        principalTable: "ChapterVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SubChapterVersion_Users_IdReviewer",
                        column: x => x.IdReviewer,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubChapterVersion_SubChapter_IdSubChapter",
                        column: x => x.IdSubChapter,
                        principalTable: "SubChapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SubChapterVersion_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlanReview",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdPlan = table.Column<int>(nullable: false),
                    IdReviser = table.Column<int>(nullable: false),
                    State = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    Comments = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanReview_UsersCreate",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanReview_SafetyStudyPlan",
                        column: x => x.IdPlan,
                        principalTable: "SafetyStudyPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanReview_Users",
                        column: x => x.IdReviser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanReview_UsersUpdate",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStudyPlanDetails",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdPlan = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    Promoter = table.Column<string>(nullable: true),
                    Localization = table.Column<string>(nullable: true),
                    Municipality = table.Column<string>(nullable: true),
                    ExecutionBudget = table.Column<double>(nullable: false),
                    ExecutionTimeDays = table.Column<int>(nullable: false),
                    ExecutionTimeMonths = table.Column<int>(nullable: false),
                    PSSBudget = table.Column<double>(nullable: false),
                    WorkersNumber = table.Column<int>(nullable: false),
                    OrganizationalStructure = table.Column<string>(nullable: true),
                    SituationDescription = table.Column<string>(nullable: true),
                    ActivityDescription = table.Column<string>(nullable: true),
                    AffectedServices = table.Column<bool>(nullable: false),
                    AffectedServicesDescription = table.Column<string>(nullable: true),
                    AssistanceCenters = table.Column<string>(nullable: true),
                    IdEmergencyPlanType = table.Column<int>(nullable: false),
                    EmergencyPlanDescription = table.Column<string>(nullable: true),
                    SecurityBudget = table.Column<string>(nullable: true),
                    ExecutionTime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStudyPlanDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanDetails_EmergencyPlanType_IdEmergencyPlanType",
                        column: x => x.IdEmergencyPlanType,
                        principalTable: "EmergencyPlanType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanDetails_SafetyStudyPlan_IdPlan",
                        column: x => x.IdPlan,
                        principalTable: "SafetyStudyPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStudyPlanFile",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdSafetyStudyPlan = table.Column<int>(nullable: false),
                    IdPlanFileType = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    File_data = table.Column<byte[]>(nullable: false),
                    FileSize = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStudyPlanFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanFile_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanFile_PlanFileType_IdPlanFileType",
                        column: x => x.IdPlanFileType,
                        principalTable: "PlanFileType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanFiles_SafetyStudyPlan",
                        column: x => x.IdSafetyStudyPlan,
                        principalTable: "SafetyStudyPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanFile_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStudyPlanPlane",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdSafetyStudyPlan = table.Column<int>(nullable: false),
                    IdPlane = table.Column<int>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FamilyName = table.Column<string>(nullable: true),
                    ContainsFile = table.Column<bool>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStudyPlanPlane", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanPlane_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanPlane_Plane_IdPlane",
                        column: x => x.IdPlane,
                        principalTable: "Plane",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanPlane_SafetyStudyPlan_IdSafetyStudyPlan",
                        column: x => x.IdSafetyStudyPlan,
                        principalTable: "SafetyStudyPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanPlane_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RisksAndPreventiveMeasures",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChapterId = table.Column<int>(nullable: false),
                    SubChapterId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    RiskId = table.Column<int>(nullable: false),
                    ProbabilityId = table.Column<int>(nullable: false),
                    SeriousnessId = table.Column<int>(nullable: false),
                    RiskLevelId = table.Column<int>(nullable: false),
                    RiskOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RisksAndPreventiveMeasures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_Chapter_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_Probability_ProbabilityId",
                        column: x => x.ProbabilityId,
                        principalTable: "Probability",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_RiskLevel_RiskLevelId",
                        column: x => x.RiskLevelId,
                        principalTable: "RiskLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_Seriousness_SeriousnessId",
                        column: x => x.SeriousnessId,
                        principalTable: "Seriousness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RisksAndPreventiveMeasures_SubChapter_SubChapterId",
                        column: x => x.SubChapterId,
                        principalTable: "SubChapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ActivityVersion",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdActivity = table.Column<int>(nullable: false),
                    IdSubChapterVersion = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Description = table.Column<string>(unicode: false, nullable: true),
                    WordDescription = table.Column<string>(nullable: true),
                    WorkDetails = table.Column<string>(nullable: true),
                    WorkOrganization = table.Column<string>(nullable: true),
                    RiskEvaluation = table.Column<string>(nullable: true),
                    MachineTool = table.Column<string>(nullable: true),
                    AssociatedDetails = table.Column<string>(nullable: true),
                    SupportFacilities = table.Column<string>(nullable: true),
                    IdReviewer = table.Column<int>(nullable: true),
                    RelationsId = table.Column<int>(nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdApprover = table.Column<int>(nullable: true),
                    ApprovementDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityVersion_UsersCreate",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityVersion_Activity",
                        column: x => x.IdActivity,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityVersion_UsersAprove",
                        column: x => x.IdApprover,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityVersion_UsersReview",
                        column: x => x.IdReviewer,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityVersion_SubChapterVersion_IdSubChapterVersion",
                        column: x => x.IdSubChapterVersion,
                        principalTable: "SubChapterVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ActivityVersion_UsersUpdate",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStudyPlanPlaneFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Data_Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER ROWGUIDCOL UNIQUE", nullable: false),
                    Data = table.Column<byte[]>(nullable: true),
                    Record_Id = table.Column<int>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SafetyStudyPlanPlaneId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStudyPlanPlaneFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanPlaneFile_SafetyStudyPlanPlane_SafetyStudyPlanPlaneId",
                        column: x => x.SafetyStudyPlanPlaneId,
                        principalTable: "SafetyStudyPlanPlane",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RiskAndPreventiveMeasuresMeasures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RisksAndPreventiveMeasuresId = table.Column<int>(nullable: false),
                    PreventiveMeasureId = table.Column<int>(nullable: false),
                    PreventiveMeasureOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskAndPreventiveMeasuresMeasures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskAndPreventiveMeasuresMeasures_PreventiveMeasure_PreventiveMeasureId",
                        column: x => x.PreventiveMeasureId,
                        principalTable: "PreventiveMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RiskAndPreventiveMeasuresMeasures_RisksAndPreventiveMeasures_RisksAndPreventiveMeasuresId",
                        column: x => x.RisksAndPreventiveMeasuresId,
                        principalTable: "RisksAndPreventiveMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlanActivityVersion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdActivityVersion = table.Column<int>(nullable: true),
                    CustomActivityId = table.Column<int>(nullable: true),
                    IdPlan = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    WordDescription = table.Column<string>(nullable: true),
                    SubChapterPosition = table.Column<int>(nullable: false),
                    ChapterPosition = table.Column<int>(nullable: false),
                    ChapterDescription = table.Column<string>(nullable: true),
                    SubChapterDescription = table.Column<string>(nullable: true),
                    AvailableActivitiId = table.Column<int>(nullable: false),
                    SubChaptId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanActivityVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanActivityVersion_CustomActivity_CustomActivityId",
                        column: x => x.CustomActivityId,
                        principalTable: "CustomActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanActivityVersion_ActivityVersion_IdActivityVersion",
                        column: x => x.IdActivityVersion,
                        principalTable: "ActivityVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanActivityVersion_SafetyStudyPlan_IdPlan",
                        column: x => x.IdPlan,
                        principalTable: "SafetyStudyPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_CreatedBy",
                table: "Activity",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ModifiedBy",
                table: "Activity",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_SubChapterId",
                table: "Activity",
                column: "SubChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityVersion_CreatedBy",
                table: "ActivityVersion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityVersion_IdActivity",
                table: "ActivityVersion",
                column: "IdActivity");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityVersion_IdApprover",
                table: "ActivityVersion",
                column: "IdApprover");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityVersion_IdReviewer",
                table: "ActivityVersion",
                column: "IdReviewer");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityVersion_IdSubChapterVersion",
                table: "ActivityVersion",
                column: "IdSubChapterVersion");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityVersion_ModifiedBy",
                table: "ActivityVersion",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AffiliatedCompany_CreatedBy",
                table: "AffiliatedCompany",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AffiliatedCompany_ModifiedBy",
                table: "AffiliatedCompany",
                column: "ModifiedBy");

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
                name: "IX_BusinessAddress_CreatedBy",
                table: "BusinessAddress",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessAddress_ModifiedBy",
                table: "BusinessAddress",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_CreatedBy",
                table: "Chapter",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_ModifiedBy",
                table: "Chapter",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterVersion_CreatedBy",
                table: "ChapterVersion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterVersion_IdApprover",
                table: "ChapterVersion",
                column: "IdApprover");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterVersion_IdChapter",
                table: "ChapterVersion",
                column: "IdChapter");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterVersion_IdReviewer",
                table: "ChapterVersion",
                column: "IdReviewer");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterVersion_ModifiedBy",
                table: "ChapterVersion",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomActivity_CustomSubchapterId",
                table: "CustomActivity",
                column: "CustomSubchapterId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomActivity_SubChapterId",
                table: "CustomActivity",
                column: "SubChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CreatedBy",
                table: "Customer",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ModifiedBy",
                table: "Customer",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomSubchapter_ChapterId",
                table: "CustomSubchapter",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomSubchapter_CustomChapterId",
                table: "CustomSubchapter",
                column: "CustomChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegation_BusinessAddressId",
                table: "Delegation",
                column: "BusinessAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegation_CreatedBy",
                table: "Delegation",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Delegation_ModifiedBy",
                table: "Delegation",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralActivity_CreatedBy",
                table: "GeneralActivity",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralActivity_ModifiedBy",
                table: "GeneralActivity",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlanActivityVersion_CustomActivityId",
                table: "PlanActivityVersion",
                column: "CustomActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanActivityVersion_IdActivityVersion",
                table: "PlanActivityVersion",
                column: "IdActivityVersion");

            migrationBuilder.CreateIndex(
                name: "IX_PlanActivityVersion_IdPlan",
                table: "PlanActivityVersion",
                column: "IdPlan");

            migrationBuilder.CreateIndex(
                name: "IX_Plane_CreatedBy",
                table: "Plane",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Plane_FamilyId",
                table: "Plane",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Plane_ModifiedBy",
                table: "Plane",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlaneFamily_CreatedBy",
                table: "PlaneFamily",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlaneFamily_ModifiedBy",
                table: "PlaneFamily",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlanReview_CreatedBy",
                table: "PlanReview",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlanReview_IdPlan",
                table: "PlanReview",
                column: "IdPlan");

            migrationBuilder.CreateIndex(
                name: "IX_PlanReview_IdReviser",
                table: "PlanReview",
                column: "IdReviser");

            migrationBuilder.CreateIndex(
                name: "IX_PlanReview_ModifiedBy",
                table: "PlanReview",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlanType_CreatedBy",
                table: "PlanType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlanType_ModifiedBy",
                table: "PlanType",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMeasure_CreatedBy",
                table: "PreventiveMeasure",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMeasure_ModifiedBy",
                table: "PreventiveMeasure",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAndPreventiveMeasuresMeasures_PreventiveMeasureId",
                table: "RiskAndPreventiveMeasuresMeasures",
                column: "PreventiveMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAndPreventiveMeasuresMeasures_RisksAndPreventiveMeasuresId",
                table: "RiskAndPreventiveMeasuresMeasures",
                column: "RisksAndPreventiveMeasuresId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskLevelBySeriousnessAndProbabilities_ProbabilityId",
                table: "RiskLevelBySeriousnessAndProbabilities",
                column: "ProbabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskLevelBySeriousnessAndProbabilities_RiskLevelId",
                table: "RiskLevelBySeriousnessAndProbabilities",
                column: "RiskLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskLevelBySeriousnessAndProbabilities_SeriousnessId",
                table: "RiskLevelBySeriousnessAndProbabilities",
                column: "SeriousnessId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_ActivityId",
                table: "RisksAndPreventiveMeasures",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_ChapterId",
                table: "RisksAndPreventiveMeasures",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_ProbabilityId",
                table: "RisksAndPreventiveMeasures",
                column: "ProbabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_RiskId",
                table: "RisksAndPreventiveMeasures",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_RiskLevelId",
                table: "RisksAndPreventiveMeasures",
                column: "RiskLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_SeriousnessId",
                table: "RisksAndPreventiveMeasures",
                column: "SeriousnessId");

            migrationBuilder.CreateIndex(
                name: "IX_RisksAndPreventiveMeasures_SubChapterId",
                table: "RisksAndPreventiveMeasures",
                column: "SubChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_CreatedBy",
                table: "SafetyStudyPlan",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdAffiliatedCompany",
                table: "SafetyStudyPlan",
                column: "IdAffiliatedCompany");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdBudget",
                table: "SafetyStudyPlan",
                column: "IdBudget");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdBusinessAddress",
                table: "SafetyStudyPlan",
                column: "IdBusinessAddress");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdCreatorProfile",
                table: "SafetyStudyPlan",
                column: "IdCreatorProfile");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdCustomer",
                table: "SafetyStudyPlan",
                column: "IdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdDelegation",
                table: "SafetyStudyPlan",
                column: "IdDelegation");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdGeneralActivity",
                table: "SafetyStudyPlan",
                column: "IdGeneralActivity");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdPlanType",
                table: "SafetyStudyPlan",
                column: "IdPlanType");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdReviewer",
                table: "SafetyStudyPlan",
                column: "IdReviewer");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_IdTemplate",
                table: "SafetyStudyPlan",
                column: "IdTemplate");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_ModifiedBy",
                table: "SafetyStudyPlan",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlan_UserId",
                table: "SafetyStudyPlan",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanDetails_CreatedBy",
                table: "SafetyStudyPlanDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanDetails_IdEmergencyPlanType",
                table: "SafetyStudyPlanDetails",
                column: "IdEmergencyPlanType");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanDetails_IdPlan",
                table: "SafetyStudyPlanDetails",
                column: "IdPlan");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanDetails_ModifiedBy",
                table: "SafetyStudyPlanDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanFile_CreatedBy",
                table: "SafetyStudyPlanFile",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanFile_IdPlanFileType",
                table: "SafetyStudyPlanFile",
                column: "IdPlanFileType");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanFile_IdSafetyStudyPlan",
                table: "SafetyStudyPlanFile",
                column: "IdSafetyStudyPlan");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanFile_ModifiedBy",
                table: "SafetyStudyPlanFile",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlane_CreatedBy",
                table: "SafetyStudyPlanPlane",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlane_IdPlane",
                table: "SafetyStudyPlanPlane",
                column: "IdPlane");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlane_IdSafetyStudyPlan",
                table: "SafetyStudyPlanPlane",
                column: "IdSafetyStudyPlan");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlane_ModifiedBy",
                table: "SafetyStudyPlanPlane",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlaneFile_Record_Id",
                table: "SafetyStudyPlanPlaneFile",
                column: "Record_Id",
                unique: true,
                filter: "[Record_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStudyPlanPlaneFile_SafetyStudyPlanPlaneId",
                table: "SafetyStudyPlanPlaneFile",
                column: "SafetyStudyPlanPlaneId");

            migrationBuilder.CreateIndex(
                name: "IX_SubChapter_CreatedBy",
                table: "SubChapter",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubChapter_IdChapter",
                table: "SubChapter",
                column: "IdChapter");

            migrationBuilder.CreateIndex(
                name: "IX_SubChapter_ModifiedBy",
                table: "SubChapter",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubChapterVersion_CreatedBy",
                table: "SubChapterVersion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubChapterVersion_IdApprover",
                table: "SubChapterVersion",
                column: "IdApprover");

            migrationBuilder.CreateIndex(
                name: "IX_SubChapterVersion_IdChapterVersion",
                table: "SubChapterVersion",
                column: "IdChapterVersion");

            migrationBuilder.CreateIndex(
                name: "IX_SubChapterVersion_IdReviewer",
                table: "SubChapterVersion",
                column: "IdReviewer");

            migrationBuilder.CreateIndex(
                name: "IX_SubChapterVersion_IdSubChapter",
                table: "SubChapterVersion",
                column: "IdSubChapter");

            migrationBuilder.CreateIndex(
                name: "IX_SubChapterVersion_ModifiedBy",
                table: "SubChapterVersion",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedBy",
                table: "Tasks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ModifiedBy",
                table: "Tasks",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Template_CreatedBy",
                table: "Template",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Template_ModifiedBy",
                table: "Template",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserChapterVersion_ChapterVersionId",
                table: "UserChapterVersion",
                column: "ChapterVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChapterVersion_UserId",
                table: "UserChapterVersion",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityRelations");

            migrationBuilder.DropTable(
                name: "ArticleTaskDetail");

            migrationBuilder.DropTable(
                name: "BudgetDetail");

            migrationBuilder.DropTable(
                name: "DefaultSafetyStudyPlanFile");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "PlanActivityVersion");

            migrationBuilder.DropTable(
                name: "PlanDetailsDefaultValues");

            migrationBuilder.DropTable(
                name: "PlanReview");

            migrationBuilder.DropTable(
                name: "RiskAndPreventiveMeasuresMeasures");

            migrationBuilder.DropTable(
                name: "RiskLevelBySeriousnessAndProbabilities");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SafetyStudyPlanDetails");

            migrationBuilder.DropTable(
                name: "SafetyStudyPlanFile");

            migrationBuilder.DropTable(
                name: "SafetyStudyPlanPlaneFile");

            migrationBuilder.DropTable(
                name: "UserChapterVersion");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "CustomActivity");

            migrationBuilder.DropTable(
                name: "ActivityVersion");

            migrationBuilder.DropTable(
                name: "PreventiveMeasure");

            migrationBuilder.DropTable(
                name: "RisksAndPreventiveMeasures");

            migrationBuilder.DropTable(
                name: "EmergencyPlanType");

            migrationBuilder.DropTable(
                name: "PlanFileType");

            migrationBuilder.DropTable(
                name: "SafetyStudyPlanPlane");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ArticleFamily");

            migrationBuilder.DropTable(
                name: "CustomSubchapter");

            migrationBuilder.DropTable(
                name: "SubChapterVersion");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "Probability");

            migrationBuilder.DropTable(
                name: "Risk");

            migrationBuilder.DropTable(
                name: "RiskLevel");

            migrationBuilder.DropTable(
                name: "Seriousness");

            migrationBuilder.DropTable(
                name: "Plane");

            migrationBuilder.DropTable(
                name: "SafetyStudyPlan");

            migrationBuilder.DropTable(
                name: "CustomChapter");

            migrationBuilder.DropTable(
                name: "ChapterVersion");

            migrationBuilder.DropTable(
                name: "SubChapter");

            migrationBuilder.DropTable(
                name: "PlaneFamily");

            migrationBuilder.DropTable(
                name: "AffiliatedCompany");

            migrationBuilder.DropTable(
                name: "Budget");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Delegation");

            migrationBuilder.DropTable(
                name: "GeneralActivity");

            migrationBuilder.DropTable(
                name: "PlanType");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "BusinessAddress");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
