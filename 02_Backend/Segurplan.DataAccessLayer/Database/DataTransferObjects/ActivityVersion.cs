using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public partial class ActivityVersion : AuditableTableBase {

        public ActivityVersion() {

            PlanActivityVersion = new HashSet<PlanActivityVersion>();
        }

        public int Id { get; set; }

        public int IdActivity { get; set; }

        public int IdSubChapterVersion { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public string WordDescription { get; set; }

        public string WorkDetails { get; set; }

        public string WorkOrganization { get; set; }

        public string RiskEvaluation { get; set; }

        public string MachineTool { get; set; }

        public string AssociatedDetails { get; set; }

        public string SupportFacilities { get; set; }

        public int? IdReviewer { get; set; }

        public int? RelationsId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ReviewDate { get; set; }

        public int? IdApprover { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ApprovementDate { get; set; }

        [ForeignKey("IdActivity")]
        [InverseProperty("ActivityVersion")]
        public Activity IdActivityNavigation { get; set; }

        [ForeignKey("IdSubChapterVersion")]
        [InverseProperty("ActivityVersion")]
        public SubChapterVersion IdSubChapterVersionNavigation { get; set; }

        [ForeignKey("IdApprover")]
        [InverseProperty("ActivityVersionIdApproverNavigation")]
        public User AprovedByNavigation { get; set; }

        [ForeignKey("IdReviewer")]
        [InverseProperty("ActivityVersionIdReviewerNavigation")]
        public User IdReviewerNavigation { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("ActivityVersionCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("ActivityVersionModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }

        [InverseProperty("IdActivityVersionNavigation")]
        public ICollection<PlanActivityVersion> PlanActivityVersion { get; set; }
    }
}
