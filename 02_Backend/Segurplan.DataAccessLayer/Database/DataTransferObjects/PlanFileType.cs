using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class PlanFileType {
        public PlanFileType() {
            SafetyStudyPlanFile = new HashSet<SafetyStudyPlanFile>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [InverseProperty("IdPlanFileTypeNavigation")]
        public ICollection<SafetyStudyPlanFile> SafetyStudyPlanFile { get; set; }
    }
}
