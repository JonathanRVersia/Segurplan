using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public partial class BusinessAddress : AuditableTableBase {
        public BusinessAddress() {
            SafetyStudyPlan = new HashSet<SafetyStudyPlan>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("BusinessAddressCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("BusinessAddressModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [InverseProperty("IdBusinessAddressNavigation")]
        public ICollection<SafetyStudyPlan> SafetyStudyPlan { get; set; }



        public ICollection<Delegation> Delegations { get; set; }

    }
}
