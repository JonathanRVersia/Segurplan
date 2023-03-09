using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public partial class PlanReview : AuditableTableBase {
        
        public int Id { get; set; }
        
        public int IdPlan { get; set; }
        
        public int IdReviser { get; set; }
        
        [Required]
        [StringLength(20)]
        public string State { get; set; }
        
        public string Comments { get; set; }
        
        [ForeignKey("CreatedBy")]
        [InverseProperty("PlanReviewCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        
        [ForeignKey("IdPlan")]
        [InverseProperty("PlanReview")]
        public SafetyStudyPlan IdPlanNavigation { get; set; }
        
        [ForeignKey("IdReviser")]
        [InverseProperty("PlanReviewIdReviserNavigation")]
        public User IdReviserNavigation { get; set; }
        
        [ForeignKey("ModifiedBy")]
        [InverseProperty("PlanReviewModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
    }
}
