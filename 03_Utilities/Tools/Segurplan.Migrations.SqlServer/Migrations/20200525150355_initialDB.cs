using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Segurplan.Migrations.SqlServer.Migrations
{
    public partial class initialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChapterVersionInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VersionNumber = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CreationUser = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationUser = table.Column<int>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterVersionInfo", x => x.Id);
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
                    AccessFailedCount = table.Column<int>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AffiliatedCompany",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffiliatedCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffiliatedCompany_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AffiliatedCompany_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    WordDescription = table.Column<string>(nullable: true),
                    IdVersionInfo = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapter_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chapter_ChapterVersionInfo_IdVersionInfo",
                        column: x => x.IdVersionInfo,
                        principalTable: "ChapterVersionInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chapter_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneralActivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralActivity_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralActivity_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreventiveMeasure",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreventiveMeasure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreventiveMeasure_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreventiveMeasure_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Notes = table.Column<string>(unicode: false, nullable: true),
                    FilePath = table.Column<string>(unicode: false, nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Delegation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    IdBusinessAddress = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delegation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delegation_UsersCreate",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Delegation_BusinessAddress_IdBusinessAddress",
                        column: x => x.IdBusinessAddress,
                        principalTable: "BusinessAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Delegation_UsersUpdate",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChapterVersion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdChapter = table.Column<int>(nullable: false),
                    IdVersionInfo = table.Column<int>(nullable: false),
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
                    ApprovementDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterVersion_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChapterVersion_Users_IdReviewer",
                        column: x => x.IdReviewer,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChapterVersion_ChapterVersionInfo_IdVersionInfo",
                        column: x => x.IdVersionInfo,
                        principalTable: "ChapterVersionInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChapterVersion_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubChapter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdChapter = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubChapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubChapter_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubChapter_Chapter_IdChapter",
                        column: x => x.IdChapter,
                        principalTable: "Chapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubChapter_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStudyPlan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdDelegation = table.Column<int>(nullable: false),
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
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_Profile_IdCreatorProfile",
                        column: x => x.IdCreatorProfile,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_Customer_IdCustomer",
                        column: x => x.IdCustomer,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_Delegation_IdDelegation",
                        column: x => x.IdDelegation,
                        principalTable: "Delegation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_GeneralActivity_IdGeneralActivity",
                        column: x => x.IdGeneralActivity,
                        principalTable: "GeneralActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlan_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubChapterId = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Description = table.Column<string>(unicode: false, nullable: false),
                    WordDescription = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activity_SubChapter",
                        column: x => x.SubChapterId,
                        principalTable: "SubChapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubChapterVersion",
                columns: table => new
                {
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
                    ApprovementDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubChapterVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubChapterVersion_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubChapterVersion_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanReview",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdPlan = table.Column<int>(nullable: false),
                    IdReviser = table.Column<int>(nullable: false),
                    State = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    Comments = table.Column<string>(unicode: false, nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStudyPlanDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdPlan = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    Promoter = table.Column<string>(nullable: true),
                    Localization = table.Column<string>(nullable: true),
                    ExecutionBudget = table.Column<double>(nullable: false),
                    ExecutionTimeDays = table.Column<int>(nullable: false),
                    ExecutionTimeMonths = table.Column<int>(nullable: false),
                    PSSBudget = table.Column<double>(nullable: false),
                    MaximumWorkers = table.Column<int>(nullable: false),
                    WorkerAverage = table.Column<double>(nullable: false),
                    OrganizationStructure = table.Column<int>(nullable: false),
                    SituationDescription = table.Column<string>(nullable: true),
                    ActivityDescription = table.Column<string>(nullable: true),
                    AffectedServices = table.Column<string>(nullable: true),
                    AssistanceCenters = table.Column<string>(nullable: true),
                    EmergencyPlan = table.Column<string>(nullable: true),
                    SecurityBudget = table.Column<string>(nullable: true),
                    ExecutionTime = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStudyPlanDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanDetails_SafetyStudyPlan_IdPlan",
                        column: x => x.IdPlan,
                        principalTable: "SafetyStudyPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStudyPlanFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdSafetyStudyPlan = table.Column<int>(nullable: false),
                    IdPlanFileType = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    File_data = table.Column<byte[]>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStudyPlanFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanFile_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanFile_PlanFileType_IdPlanFileType",
                        column: x => x.IdPlanFileType,
                        principalTable: "PlanFileType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanFiles_SafetyStudyPlan",
                        column: x => x.IdSafetyStudyPlan,
                        principalTable: "SafetyStudyPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SafetyStudyPlanFile_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityVersion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdActivity = table.Column<int>(nullable: false),
                    IdSubChapterVersion = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Description = table.Column<string>(unicode: false, nullable: true),
                    WordDescription = table.Column<string>(nullable: true),
                    WorkOrganization = table.Column<string>(nullable: true),
                    RiskEvaluation = table.Column<string>(nullable: true),
                    MachineTool = table.Column<string>(nullable: true),
                    AssociatedDetails = table.Column<string>(nullable: true),
                    SupportFacilities = table.Column<string>(nullable: true),
                    IdReviewer = table.Column<int>(nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdApprover = table.Column<int>(nullable: true),
                    ApprovementDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityVersion_UsersUpdate",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanActivityVersion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdActivityVersion = table.Column<int>(nullable: false),
                    IdPlan = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    WordDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanActivityVersion", x => x.Id);
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
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Chapter_IdVersionInfo",
                table: "Chapter",
                column: "IdVersionInfo");

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
                name: "IX_ChapterVersion_IdVersionInfo",
                table: "ChapterVersion",
                column: "IdVersionInfo");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterVersion_ModifiedBy",
                table: "ChapterVersion",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CreatedBy",
                table: "Customer",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ModifiedBy",
                table: "Customer",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Delegation_CreatedBy",
                table: "Delegation",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Delegation_IdBusinessAddress",
                table: "Delegation",
                column: "IdBusinessAddress");

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
                name: "IX_PlanActivityVersion_IdActivityVersion",
                table: "PlanActivityVersion",
                column: "IdActivityVersion");

            migrationBuilder.CreateIndex(
                name: "IX_PlanActivityVersion_IdPlan",
                table: "PlanActivityVersion",
                column: "IdPlan");

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
                name: "IX_Template_CreatedBy",
                table: "Template",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Template_ModifiedBy",
                table: "Template",
                column: "ModifiedBy");

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
                name: "PlanActivityVersion");

            migrationBuilder.DropTable(
                name: "PlanReview");

            migrationBuilder.DropTable(
                name: "PreventiveMeasure");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SafetyStudyPlanDetails");

            migrationBuilder.DropTable(
                name: "SafetyStudyPlanFile");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "ActivityVersion");

            migrationBuilder.DropTable(
                name: "PlanFileType");

            migrationBuilder.DropTable(
                name: "SafetyStudyPlan");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "SubChapterVersion");

            migrationBuilder.DropTable(
                name: "AffiliatedCompany");

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
                name: "ChapterVersion");

            migrationBuilder.DropTable(
                name: "SubChapter");

            migrationBuilder.DropTable(
                name: "BusinessAddress");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ChapterVersionInfo");
        }
    }
}
