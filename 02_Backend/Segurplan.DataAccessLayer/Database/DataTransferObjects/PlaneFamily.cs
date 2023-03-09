using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class PlaneFamily : AuditableTableBase {

        public PlaneFamily() {
            Plane = new HashSet<Plane>();
        }

        public int Id { get; set; }

        public string Family { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("PlanFamilyCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("PlanFamilyModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [InverseProperty("PlaneFamilyIdNavigation")]
        public ICollection<Plane> Plane { get; set; }
    }
}
