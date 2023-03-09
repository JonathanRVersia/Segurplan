using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public partial class SafetyStudyPlan : AuditableTableBase {
        public SafetyStudyPlan() {
            PlanReview = new HashSet<PlanReview>();
            SafetyStudyPlanDetails = new HashSet<SafetyStudyPlanDetails>();
            //SafetyStudyPlanFile = new HashSet<SafetyStudyPlanFile>();
            PlanActivityVersion = new HashSet<PlanActivityVersion>();
            SafetyStudyPlanPlane = new HashSet<SafetyStudyPlanPlane>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IdDelegation { get; set; }

        public int IdBudget { get; set; }

        [Required]
        public int IdBusinessAddress { get; set; }

        public int IdAffiliatedCompany { get; set; }

        [StringLength(250)]
        public string ProjectName { get; set; }

        public int IdCustomer { get; set; }

        [StringLength(250)]
        public string PlanCustomerDescription { get; set; }

        public int IdGeneralActivity { get; set; }

        public int IdPlanType { get; set; }

        public int IdTemplate { get; set; }

        public string ApproverName { get; set; }

        public int IdReviewer { get; set; }

        public string CreatorName { get; set; }

        public int IdCreatorProfile { get; set; }

        //TODO: Se debe actualizar el modelo al momento de realizar la 
        //public int IdBudget { get; set; }

        //public int CreatedBy { get; set; }

        //[Column(TypeName = "datetime")]
        //public DateTime CreateDate { get; set; }

        //public int ModifiedBy { get; set; }

        //[Column(TypeName = "datetime")]
        //public DateTime UpdateDate { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SafetyStudyPlanCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("IdReviewer")]
        [InverseProperty("SafetyStudyPlanIdReviewerNavigation")]
        public User IdReviewerNavigation { get; set; }

        [ForeignKey("IdGeneralActivity")]
        [InverseProperty("SafetyStudyPlan")]
        public GeneralActivity IdGeneralActivityNavigation { get; set; }

        [ForeignKey("IdDelegation")]
        [InverseProperty("SafetyStudyPlan")]
        public Delegation IdDelegationNavigation { get; set; }

        [ForeignKey("IdBusinessAddress")]
        [InverseProperty("SafetyStudyPlan")]
        public BusinessAddress IdBusinessAddressNavigation { get; set; }

        [ForeignKey("IdAffiliatedCompany")]
        [InverseProperty("SafetyStudyPlan")]
        public AffiliatedCompany IdAffiliatedCompanyNavigation { get; set; }

        [ForeignKey("IdCustomer")]
        [InverseProperty("SafetyStudyPlan")]
        public Customer IdCustomerNavigation { get; set; }

        [ForeignKey("IdCreatorProfile")]
        [InverseProperty("SafetyStudyPlan")]
        public Profile IdCreatorProfileNavigation { get; set; }

        [ForeignKey("IdPlanType")]
        [InverseProperty("SafetyStudyPlan")]
        public PlanType IdPlanTypeNavigation { get; set; }

        [ForeignKey("IdTemplate")]
        [InverseProperty("SafetyStudyPlan")]
        public Template IdTemplateNavigation { get; set; }
        [ForeignKey("IdBudget")]
        [InverseProperty("SafetyStudyPlan")]
        public Budget IdBudgetNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("SafetyStudyPlanModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [InverseProperty("IdPlanNavigation")]
        public ICollection<PlanReview> PlanReview { get; set; }

        [InverseProperty("IdPlanNavigation")]
        public ICollection<SafetyStudyPlanDetails> SafetyStudyPlanDetails { get; set; }

        [InverseProperty("IdSafetyStudyPlanNavigation")]
        public List<SafetyStudyPlanFile> SafetyStudyPlanFile { get; set; } = new List<SafetyStudyPlanFile>();

        [InverseProperty("IdPlanNavigation")]
        public ICollection<PlanActivityVersion> PlanActivityVersion { get; set; }

        [InverseProperty("SafetyStudyPlanPlaneIPlanNavigation")]
        public ICollection<SafetyStudyPlanPlane> SafetyStudyPlanPlane { get; set; }
    }
}
