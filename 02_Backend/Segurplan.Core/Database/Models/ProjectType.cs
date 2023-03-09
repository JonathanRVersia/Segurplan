using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.Core.Domain.Identity;

namespace Segurplan.Core.Database.Models {
    public partial class ProjectType {
        public ProjectType() {
            SafetyStudyPlan = new HashSet<SafetyStudyPlan>();
            SafetyStudyPlanDetails = new HashSet<SafetyStudyPlanDetails>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("ProjectTypeCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("ProjectTypeModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        [InverseProperty("IdProjectTypeNavigation")]
        public ICollection<SafetyStudyPlan> SafetyStudyPlan { get; set; }
        [InverseProperty("IdProjectTypeNavigation")]
        public ICollection<SafetyStudyPlanDetails> SafetyStudyPlanDetails { get; set; }
    }
}
