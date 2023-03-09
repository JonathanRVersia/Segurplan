using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class Plane : AuditableTableBase {

        public Plane() {

            SafetyStudyPlanPlane = new HashSet<SafetyStudyPlanPlane>();
        }

        public int Id { get; set; }

        public int FamilyId { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public bool IsAvailable { get; set; }

        public byte[] Data { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("PlaneCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("PlaneModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [ForeignKey("FamilyId")]
        [InverseProperty("Plane")]
        public PlaneFamily PlaneFamilyIdNavigation { get; set; }

        [InverseProperty("SafetyStudyPlanPlaneIdPlaneNavigation")]
        public ICollection<SafetyStudyPlanPlane> SafetyStudyPlanPlane { get; set; }

    }
}
