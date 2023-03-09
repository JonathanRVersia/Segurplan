using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public partial class SubChapterVersion : AuditableTableBase {

        public SubChapterVersion() {
            ActivityVersion = new HashSet<ActivityVersion>();
        }

        public int Id { get; set; }

        public int IdSubChapter { get; set; }

        public int IdChapterVersion { get; set; }

        public int Number { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string WorkDetails { get; set; }

        public string WorkOrganization { get; set; }

        public string RiskEvaluation { get; set; }

        public string MachineTool { get; set; }

        public string AssociatedDetails { get; set; }

        public string SupportFacilities { get; set; }

        public int? IdReviewer { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ReviewDate { get; set; }

        public int? IdApprover { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ApprovementDate { get; set; }

        [ForeignKey("IdSubChapter")]
        [InverseProperty("SubChapterVersion")]
        public SubChapter IdSubChapterNavigation { get; set; }

        [ForeignKey("IdChapterVersion")]
        [InverseProperty("SubChapterVersion")]
        public ChapterVersion IdChapterVersionNavigation { get; set; }

        [ForeignKey("IdApprover")]
        [InverseProperty("SubChapterVersionIdApproverNavigation")]
        public User AprovedByNavigation { get; set; }

        [ForeignKey("IdReviewer")]
        [InverseProperty("SubChapterVersionIdReviewerNavigation")]
        public User IdReviewerNavigation { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SubChapterVersionCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("SubChapterVersionModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [InverseProperty("IdSubChapterVersionNavigation")]
        public ICollection<ActivityVersion> ActivityVersion { get; set; }
    }
}
