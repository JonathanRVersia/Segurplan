using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class SafetyStudyPlanPlane : AuditableTableBase {

        public int Id { get; set; }

        public int IdSafetyStudyPlan { get; set; }

        public int? IdPlane { get; set; }

        public int Position { get; set; }

        public string Description { get; set; }

        public string FamilyName { get; set; }

        public bool ContainsFile { get; set; }

        public bool IsAvailable { get; set; }

        public List<SafetyStudyPlanPlaneFile> Files { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SafetyStudyPlanPlaneCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("SafetyStudyPlanPlaneModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [ForeignKey("IdSafetyStudyPlan")]
        [InverseProperty("SafetyStudyPlanPlane")]
        public SafetyStudyPlan SafetyStudyPlanPlaneIPlanNavigation { get; set; }

        [ForeignKey("IdPlane")]
        [InverseProperty("SafetyStudyPlanPlane")]
        public Plane SafetyStudyPlanPlaneIdPlaneNavigation { get; set; }
    }
}
