using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public partial class Activity : AuditableTableBase {
        public Activity() {
            ActivityVersion = new HashSet<ActivityVersion>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SubChapterId { get; set; }

        public int Number { get; set; }

        [Required]
        public string Description { get; set; }

        public string WordDescription { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("ActivityCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("ActivityModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [ForeignKey("SubChapterId")]
        [InverseProperty("Activity")]
        public SubChapter SubChapter { get; set; }

        [InverseProperty("IdActivityNavigation")]
        public ICollection<ActivityVersion> ActivityVersion { get; set; }
    }
}
