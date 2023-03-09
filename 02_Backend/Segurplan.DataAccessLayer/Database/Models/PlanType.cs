using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.Core.Database.Models {
    public partial class PlanType {
        public PlanType() {
            SafetyStudyPlan = new HashSet<SafetyStudyPlan>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }

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
