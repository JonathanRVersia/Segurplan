using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public partial class SubChapter : AuditableTableBase {
        public SubChapter() {
            Activity = new HashSet<Activity>();
            SubChapterVersion = new HashSet<SubChapterVersion>();
        }

        public int Id { get; set; }

        public int IdChapter { get; set; }

        public int Number { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SubChapterCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("SubChapterModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [ForeignKey("IdChapter")]
        [InverseProperty("SubChapter")]
        public Chapter IdChapterNavigation { get; set; }

        [InverseProperty("SubChapter")]
        public ICollection<Activity> Activity { get; set; }

        public ICollection<SubChapterVersion> SubChapterVersion { get; set; }
    }
}
