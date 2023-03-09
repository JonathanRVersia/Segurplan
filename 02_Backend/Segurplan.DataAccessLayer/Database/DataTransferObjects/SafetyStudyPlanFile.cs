using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    [Table("SafetyStudyPlanFile")]
    public partial class SafetyStudyPlanFile : AuditableTableBase {

        private DateTime timeStamp;

        public int Id { get; set; }

        public int IdSafetyStudyPlan { get; set; }

        public int IdPlanFileType { get; set; }

        [Required]
        [StringLength(100)]
        public string FileName { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime TimeStamp {
            get {
                if (timeStamp == null || timeStamp == DateTime.MinValue) {
                    timeStamp = DateTime.Now;
                }

                return timeStamp;
            }
            set {
                timeStamp = value;
            }
        }

        [Required]
        [Column("File_data")]
        public byte[] FileData { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal FileSize { get; set; }
        

        [ForeignKey("CreatedBy")]
        [InverseProperty("SafetyStudyPlanFileCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("SafetyStudyPlanFileModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [ForeignKey("IdSafetyStudyPlan")]
        [InverseProperty("SafetyStudyPlanFile")]
        public SafetyStudyPlan IdSafetyStudyPlanNavigation { get; set; }

        [ForeignKey("IdPlanFileType")]
        [InverseProperty("SafetyStudyPlanFile")]
        public PlanFileType IdPlanFileTypeNavigation { get; set; }
    }
}
