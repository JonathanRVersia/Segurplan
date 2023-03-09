using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public partial class Chapter : AuditableTableBase {
        public Chapter() {
            ChapterVersion = new HashSet<ChapterVersion>();
            SubChapter = new HashSet<SubChapter>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Number { get; set; }

        [Required]
        public string Title { get; set; }

        public string WordDescription { get; set; }

        public bool DefaultSelectedChapter { get; set; }

        //public int IdVersionInfo { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("ChapterCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("ChapterModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        //[ForeignKey("IdVersionInfo")]
        //[InverseProperty("Chapter")]
        //public ChapterVersionInfo IdVersionInfoNavigation { get; set; }

        public ICollection<ChapterVersion> ChapterVersion { get; set; }

        public ICollection<SubChapter> SubChapter { get; set; }
    }
}
