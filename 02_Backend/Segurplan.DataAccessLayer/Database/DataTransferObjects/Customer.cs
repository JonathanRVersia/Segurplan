using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class Customer : AuditableTableBase {
        public Customer() {
            SafetyStudyPlan = new HashSet<SafetyStudyPlan>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CustomerCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("CustomerModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [InverseProperty("IdCustomerNavigation")]
        public ICollection<SafetyStudyPlan> SafetyStudyPlan { get; set; }
    }
}
