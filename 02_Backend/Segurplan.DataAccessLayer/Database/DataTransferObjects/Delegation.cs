using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public partial class Delegation : AuditableTableBase {
        public Delegation() {
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
        [InverseProperty("DelegationCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("DelegationModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [InverseProperty("IdDelegationNavigation")]
        public ICollection<SafetyStudyPlan> SafetyStudyPlan { get; set; }


        public int? BusinessAddressId { get; set; }
        public BusinessAddress BusinessAddress { get; set; }
    }
}
