using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public partial class PlanType : AuditableTableBase {
        public PlanType() {
            SafetyStudyPlan = new HashSet<SafetyStudyPlan>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [ForeignKey("CreatedBy")]
        [InverseProperty("PlanTypeCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        
        [ForeignKey("ModifiedBy")]
        [InverseProperty("PlanTypeModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        
        [InverseProperty("IdPlanTypeNavigation")]
        public ICollection<SafetyStudyPlan> SafetyStudyPlan { get; set; }
    }
}
