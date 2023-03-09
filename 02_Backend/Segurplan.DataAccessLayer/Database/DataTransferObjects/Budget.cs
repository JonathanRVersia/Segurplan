using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class Budget: AuditableTableBase {
        public Budget() {
            this.BudgetDetails = new HashSet<BudgetDetail>();
        }
        public int Id { get; set; }
        [Required]
        public int ApplicabePercentage { get; set; }
        [Required]
        public decimal StudyBudget { get; set; }
        [Required]
        public decimal Difference { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("BudgetCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("BudgetModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        [InverseProperty("IdBudgetNavigation")]
        public ICollection<SafetyStudyPlan> SafetyStudyPlan { get; set; }
        [InverseProperty("IdBudgetNavigation")]
        public ICollection<BudgetDetail> BudgetDetails { get; set; }
    }
}
