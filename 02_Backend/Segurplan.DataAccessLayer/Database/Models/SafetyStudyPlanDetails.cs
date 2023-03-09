using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.Core.Database.Models {
    [Table("SafetyStudyPlan_Details")]
    public partial class SafetyStudyPlanDetails {
        public int Id { get; set; }
        public int IdPlan { get; set; }
        public bool IncludeDescriptionsInWord { get; set; }
        public int? AverageOfWorkers { get; set; }
        [StringLength(250)]
        public string PromoterName { get; set; }
        public int? MaxNumOfWorkers { get; set; }
        public int IdProjectType { get; set; }
        [StringLength(250)]
        public string Anagram { get; set; }
        public bool ShowRiskAssessment { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal? Budget { get; set; }
        public int? ExecutionTime { get; set; }
        public string CompanySituation { get; set; }
        public string ImagePath { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SafetyStudyPlanDetailsCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        [ForeignKey("IdPlan")]
        [InverseProperty("SafetyStudyPlanDetails")]
        public SafetyStudyPlan IdPlanNavigation { get; set; }
        [ForeignKey("IdProjectType")]
        [InverseProperty("SafetyStudyPlanDetails")]
        public ProjectType IdProjectTypeNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("SafetyStudyPlanDetailsModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
    }
}
