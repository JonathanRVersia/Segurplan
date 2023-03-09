using System;
using System.Collections.Generic;
using MediatR;
using Segurplan.Core.Actions.Administration.ChapterDetails.Models;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Save {
    public class SaveChapterRequest : IRequest<IRequestResponse<SaveChapterResponse>> {

        public string RemoveSubChapterIds { get; set; }

        public int Id { get; set; }

        public int IdChapter { get; set; }

        public int VersionNumber { get; set; }

        //public int IdVersionInfo { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string WordDescription { get; set; }

        public string WorkDetails { get; set; }

        public string WorkOrganization { get; set; }

        public string RiskEvaluation { get; set; }

        public string MachineTool { get; set; }

        public string AssociatedDetails { get; set; }

        public string SupportFacilities { get; set; }

        public int? IdReviewer { get; set; }

        public DateTime? ReviewDate { get; set; }

        public int? IdApprover { get; set; }

        public DateTime? ApprovementDate { get; set; }

        public List<UserChapterDetails> ProducedBy { get; set; }

        //[ForeignKey("IdApprover")]
        //[InverseProperty("ChapterVersionIdApproverNavigation")]
        //public User AprovedByNavigation { get; set; }

        //[ForeignKey("CreatedBy")]
        //[InverseProperty("ChapterVersionCreatedByNavigation")]
        //public User CreatedByNavigation { get; set; }

        //[ForeignKey("ModifiedBy")]
        //[InverseProperty("ChapterVersionModifiedByNavigation")]
        //public User ModifiedByNavigation { get; set; }

        //[ForeignKey("IdReviewer")]
        //[InverseProperty("ChapterVersionIdReviewerNavigation")]
        //public User IdReviewerNavigation { get; set; }



        //[ForeignKey("IdChapter")]
        //[InverseProperty("ChapterVersion")]
        //public Chapter IdChapterNavigation { get; set; }

        //[ForeignKey("IdVersionInfo")]
        //[InverseProperty("ChapterVersion")]
        //public ChapterVersionInfo IdVersionInfoNavigation { get; set; }

        //[InverseProperty("IdChapterVersionNavigation")]
        //public ICollection<SubChapterVersion> SubChapterVersion { get; set; }
    }
}
