using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.Core.Domain.Identity;

namespace Segurplan.Core.Database.Models {
    public partial class SafetyStudyPlan {
        public SafetyStudyPlan() {
            SafetyStudyPlanDetails = new HashSet<SafetyStudyPlanDetails>();
        }

        public int Id { get; set; }
        public int IdCenter { get; set; }
        [StringLength(250)]
        public string CompanyName { get; set; }
        [StringLength(250)]
        public string ProjectName { get; set; }
        [StringLength(250)]
        public string CustomerName { get; set; }
        [StringLength(250)]
        public string PlanActivity { get; set; }
        public int IdPlanType { get; set; }
        public int IdProjectType { get; set; }
        public int IdTemplate { get; set; }
        public int? IdAprover { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SafetyStudyPlanCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        [ForeignKey("IdAprover")]
        [InverseProperty("SafetyStudyPlanIdAproverNavigation")]
        public User IdAproverNavigation { get; set; }
        [ForeignKey("IdCenter")]
        [InverseProperty("SafetyStudyPlan")]
        public Center IdCenterNavigation { get; set; }
        [ForeignKey("IdPlanType")]
        [InverseProperty("SafetyStudyPlan")]
        public PlanType IdPlanTypeNavigation { get; set; }
        [ForeignKey("IdProjectType")]
        [InverseProperty("SafetyStudyPlan")]
        public ProjectType IdProjectTypeNavigation { get; set; }
        [ForeignKey("IdTemplate")]
        [InverseProperty("SafetyStudyPlan")]
        public Template IdTemplateNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("SafetyStudyPlanModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        [InverseProperty("IdPlanNavigation")]
        public ICollection<SafetyStudyPlanDetails> SafetyStudyPlanDetails { get; set; }
    }
}
