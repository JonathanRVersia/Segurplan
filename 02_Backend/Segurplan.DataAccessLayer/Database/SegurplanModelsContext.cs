using Microsoft.EntityFrameworkCore;
using Segurplan.DataAccessLayer.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.Core.Database {
    public partial class SegurplanContext {
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<ActivityVersion> ActivityVersion { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<BusinessAddress> BusinessAddress { get; set; }
        public virtual DbSet<Chapter> Chapter { get; set; }
        public virtual DbSet<ChapterVersion> ChapterVersion { get; set; }
        public virtual DbSet<Delegation> Delegation { get; set; }
        public virtual DbSet<EmergencyPlanType> EmergencyPlanType { get; set; }
        public virtual DbSet<PlanReview> PlanReview { get; set; }
        public virtual DbSet<PlanType> PlanType { get; set; }
        public virtual DbSet<SafetyStudyPlan> SafetyStudyPlan { get; set; }
        public virtual DbSet<SafetyStudyPlanDetails> SafetyStudyPlanDetails { get; set; }
        public virtual DbSet<SafetyStudyPlanFile> SafetyStudyPlanFile { get; set; }
        public virtual DbSet<SubChapter> SubChapter { get; set; }
        public virtual DbSet<SubChapterVersion> SubChapterVersion { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<AffiliatedCompany> AffiliatedCompany { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<GeneralActivity> GeneralActivity { get; set; }
        public virtual DbSet<PreventiveMeasure> PreventiveMeasure { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<PlanActivityVersion> PlanActivityVersion { get; set; }
        //public virtual DbSet<ChapterVersionInfo> ChapterVersionInfo { get; set; }
        public virtual DbSet<UserChapterVersion> UserChapterVersion { get; set; }
        public virtual DbSet<ActivityRelations> ActivityRelations { get; set; }

        public virtual DbSet<Plane> Plane { get; set; }

        public virtual DbSet<PlaneFamily> PlaneFamily { get; set; }

        public virtual DbSet<SafetyStudyPlanPlane> SafetyStudyPlanPlane { get; set; }
        public virtual DbSet<SafetyStudyPlanPlaneFile> SafetyStudyPlanPlaneFile { get; set; }

        public virtual DbSet<RisksAndPreventiveMeasures> RisksAndPreventiveMeasures { get; set; }
        public virtual DbSet<RiskAndPreventiveMeasuresMeasures> RiskAndPreventiveMeasuresMeasures { get; set; }
        public virtual DbSet<Risk> Risk { get; set; }
        public virtual DbSet<Probability> Probability { get; set; }
        public virtual DbSet<Seriousness> Seriousness { get; set; }
        public virtual DbSet<RiskLevel> RiskLevel { get; set; }

        public virtual DbSet<CustomActivity> CustomActivity { get; set; }
        public virtual DbSet<CustomSubchapter> CustomSubchapter { get; set; }
        public virtual DbSet<CustomChapter> CustomChapter { get; set; }
        public virtual DbSet<DefaultSafetyStudyPlanFile> DefaultSafetyStudyPlanFile { get; set; }
        public virtual DbSet<PlanDetailsDefaultValues> PlanDetailsDefaultValues { get; set; }
        public virtual DbSet<RiskLevelBySeriousnessAndProbability> RiskLevelBySeriousnessAndProbabilities { get; set; }

        public virtual DbSet<ArticleFamily> ArticleFamily { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Budget> Budget { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<ArticleTaskDetail> ArticleTaskDetail { get; set; }
        public virtual DbSet<BudgetDetail> BudgetDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            entityTypeConfigurations.ApplyTo(modelBuilder);

            modelBuilder.Entity<Activity>(entity => {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ActivityCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Activity_UsersCreate");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.ActivityModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Activity_UsersUpdate");

                entity.HasOne(d => d.SubChapter)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.SubChapterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Activity_SubChapter");
            });

            modelBuilder.Entity<ActivityVersion>(entity => {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.HasOne(d => d.AprovedByNavigation)
                    .WithMany(p => p.ActivityVersionIdApproverNavigation)
                    .HasForeignKey(d => d.IdApprover)
                    .HasConstraintName("FK_ActivityVersion_UsersAprove");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ActivityVersionCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivityVersion_UsersCreate");

                entity.HasOne(d => d.IdActivityNavigation)
                    .WithMany(p => p.ActivityVersion)
                    .HasForeignKey(d => d.IdActivity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivityVersion_Activity");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.ActivityVersionModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_ActivityVersion_UsersUpdate");

                entity.HasOne(d => d.IdReviewerNavigation)
                    .WithMany(p => p.ActivityVersionIdReviewerNavigation)
                    .HasForeignKey(d => d.IdReviewer)
                    .HasConstraintName("FK_ActivityVersion_UsersReview");
            });

            modelBuilder.Entity<BusinessAddress>(entity => {
                entity.HasIndex(e => e.CreatedBy);

                entity.HasIndex(e => e.ModifiedBy);

                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.BusinessAddressCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BusinessAddress_UsersCreate");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.BusinessAddressModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_BusinessAddress_UsersUpdate");
            });

            modelBuilder.Entity<Delegation>(entity => {
                entity.HasIndex(e => e.CreatedBy);

                entity.HasIndex(e => e.ModifiedBy);

                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.DelegationCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Delegation_UsersCreate");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.DelegationModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Delegation_UsersUpdate");
            });

            modelBuilder.Entity<PlanReview>(entity => {
                entity.HasIndex(e => e.CreatedBy);

                entity.HasIndex(e => e.IdPlan);

                entity.HasIndex(e => e.IdReviser);

                entity.HasIndex(e => e.ModifiedBy);

                entity.Property(e => e.Comments).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.State).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.PlanReviewCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanReview_UsersCreate");

                entity.HasOne(d => d.IdPlanNavigation)
                    .WithMany(p => p.PlanReview)
                    .HasForeignKey(d => d.IdPlan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanReview_SafetyStudyPlan");

                entity.HasOne(d => d.IdReviserNavigation)
                    .WithMany(p => p.PlanReviewIdReviserNavigation)
                    .HasForeignKey(d => d.IdReviser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanReview_Users");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.PlanReviewModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_PlanReview_UsersUpdate");
            });

            modelBuilder.Entity<PlanType>(entity => {
                entity.HasIndex(e => e.CreatedBy);

                entity.HasIndex(e => e.ModifiedBy);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.PlanTypeCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanType_UsersCreate");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.PlanTypeModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_PlanType_UsersUpdate");
            });

            modelBuilder.Entity<SafetyStudyPlan>(entity => {
                entity.HasIndex(e => e.CreatedBy);

                entity.HasIndex(e => e.IdPlanType);

                entity.HasIndex(e => e.IdTemplate);

                entity.HasIndex(e => e.ModifiedBy);

                entity.Property(e => e.PlanCustomerDescription).IsUnicode(false);

                entity.Property(e => e.ProjectName).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SafetyStudyPlanCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SafetyStudyPlan_UsersCreate");

                entity.HasOne(d => d.IdPlanTypeNavigation)
                    .WithMany(p => p.SafetyStudyPlan)
                    .HasForeignKey(d => d.IdPlanType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SafetyStudyPlan_PlanType");

                entity.HasOne(d => d.IdTemplateNavigation)
                    .WithMany(p => p.SafetyStudyPlan)
                    .HasForeignKey(d => d.IdTemplate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SafetyStudyPlan_Template");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.SafetyStudyPlanModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_SafetyStudyPlan_UsersUpdate");
            });

            modelBuilder.Entity<SafetyStudyPlanFile>(entity => {
                entity.HasIndex(e => e.IdSafetyStudyPlan);

                entity.Property(e => e.FileName).IsUnicode(false);

                entity.HasOne(d => d.IdSafetyStudyPlanNavigation)
                    .WithMany(p => p.SafetyStudyPlanFile)
                    .HasForeignKey(d => d.IdSafetyStudyPlan)
                    .HasConstraintName("FK_SafetyStudyPlanFiles_SafetyStudyPlan");
            });

            modelBuilder.Entity<Template>(entity => {
                entity.HasIndex(e => e.CreatedBy);

                entity.HasIndex(e => e.ModifiedBy);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FilePath).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TemplateCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Template_UsersCreate");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.TemplateModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Template_UsersUpdate");
            });


            modelBuilder.Entity<SafetyStudyPlanPlaneFile>(entity => {
                entity.Property(e => e.Data_Id).HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL UNIQUE");
                entity.HasIndex(e => e.Record_Id).IsUnique();
            });

            modelBuilder.Entity<CustomChapter>(entity => {

            });

            modelBuilder.Entity<CustomSubchapter>(entity => {

            });

            modelBuilder.Entity<CustomActivity>(entity => {

            });

            modelBuilder.Entity<DefaultSafetyStudyPlanFile>(entity => { });

            modelBuilder.Entity<Plane>(entity => {
                entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            });
        }
    }
}
