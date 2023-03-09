using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database.Models;

namespace Segurplan.Core.Database {
    public partial class SegurplanContext {
        public virtual DbSet<BusinessAddress> BusinessAddress { get; set; }
        public virtual DbSet<Center> Center { get; set; }
        public virtual DbSet<Delegation> Delegation { get; set; }
        public virtual DbSet<PlanType> PlanType { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }
        public virtual DbSet<SafetyStudyPlan> SafetyStudyPlan { get; set; }
        public virtual DbSet<SafetyStudyPlanDetails> SafetyStudyPlanDetails { get; set; }
        public virtual DbSet<Template> Template { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            entityTypeConfigurations.ApplyTo(modelBuilder);

            modelBuilder.Entity<BusinessAddress>(entity => {
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

            modelBuilder.Entity<Center>(entity => {
                entity.HasIndex(e => new { e.Id, e.IdDelegation })
                    .HasName("UQ_Center")
                    .IsUnique();

                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CenterCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Center_UsersCreate");

                entity.HasOne(d => d.IdAddressNavigation)
                    .WithMany(p => p.Center)
                    .HasForeignKey(d => d.IdAddress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Center_Addresss");

                entity.HasOne(d => d.IdDelegationNavigation)
                    .WithMany(p => p.Center)
                    .HasForeignKey(d => d.IdDelegation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Center_Delegation");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.CenterModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Center_UsersUpdate");
            });

            modelBuilder.Entity<Delegation>(entity => {
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

            modelBuilder.Entity<PlanType>(entity => {
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

            modelBuilder.Entity<ProjectType>(entity => {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ProjectTypeCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectType_UsersCreate");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.ProjectTypeModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_ProjectType_UsersUpdate");
            });

            modelBuilder.Entity<SafetyStudyPlan>(entity => {
                entity.Property(e => e.CompanyName).IsUnicode(false);

                entity.Property(e => e.CustomerName).IsUnicode(false);

                entity.Property(e => e.PlanActivity).IsUnicode(false);

                entity.Property(e => e.ProjectName).IsUnicode(false);

                entity.Property(e => e.Type).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SafetyStudyPlanCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SafetyStudyPlan_UsersCreate");

                entity.HasOne(d => d.IdAproverNavigation)
                    .WithMany(p => p.SafetyStudyPlanIdAproverNavigation)
                    .HasForeignKey(d => d.IdAprover)
                    .HasConstraintName("FK_SafetyStudyPlan_Users");

                entity.HasOne(d => d.IdCenterNavigation)
                    .WithMany(p => p.SafetyStudyPlan)
                    .HasForeignKey(d => d.IdCenter)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SafetyStudyPlan_Center");

                entity.HasOne(d => d.IdPlanTypeNavigation)
                    .WithMany(p => p.SafetyStudyPlan)
                    .HasForeignKey(d => d.IdPlanType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SafetyStudyPlan_PlanType");

                entity.HasOne(d => d.IdProjectTypeNavigation)
                    .WithMany(p => p.SafetyStudyPlan)
                    .HasForeignKey(d => d.IdProjectType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SafetyStudyPlan_ProjectType");

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

            modelBuilder.Entity<SafetyStudyPlanDetails>(entity => {
                entity.Property(e => e.Anagram).IsUnicode(false);

                entity.Property(e => e.CompanySituation).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImagePath).IsUnicode(false);

                entity.Property(e => e.PromoterName).IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SafetyStudyPlanDetailsCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SafetyStudyPlan_DetailsUsersCreate");

                entity.HasOne(d => d.IdPlanNavigation)
                    .WithMany(p => p.SafetyStudyPlanDetails)
                    .HasForeignKey(d => d.IdPlan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SafetyStudyPlan_Details_SafetyStudyPlan");

                entity.HasOne(d => d.IdProjectTypeNavigation)
                    .WithMany(p => p.SafetyStudyPlanDetails)
                    .HasForeignKey(d => d.IdProjectType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SafetyStudyPlan_Details_ProjectType");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.SafetyStudyPlanDetailsModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_SafetyStudyPlan_DetailsUsersUpdate");
            });

            modelBuilder.Entity<Template>(entity => {
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
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
