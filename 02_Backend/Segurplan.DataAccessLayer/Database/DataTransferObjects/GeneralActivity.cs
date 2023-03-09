using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class GeneralActivity : AuditableTableBase {

        public GeneralActivity() {
            SafetyStudyPlan = new HashSet<SafetyStudyPlan>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("GeneralActivityCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("GeneralActivityModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [InverseProperty("IdGeneralActivityNavigation")]
        public ICollection<SafetyStudyPlan> SafetyStudyPlan { get; set; }
    }
}
