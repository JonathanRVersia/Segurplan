using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.DataAccessLayer.Enums;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public partial class Template : AuditableTableBase {

        public Template() {
            SafetyStudyPlan = new HashSet<SafetyStudyPlan>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public TemplateType TemplateType { get; set; }

        public string Notes { get; set; }

        public string FilePath { get; set; }

        //[Required]
        [Column("File_data")]
        public byte[] FileData { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal FileSize { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("TemplateCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("TemplateModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [InverseProperty("IdTemplateNavigation")]
        public ICollection<SafetyStudyPlan> SafetyStudyPlan { get; set; }
    }
}
