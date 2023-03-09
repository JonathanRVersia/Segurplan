using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Segurplan.Core.Actions.Administration.ChapterDetails.Models;

namespace Segurplan.Web.Pages.Models.Administration.ChaptersAndActivities.ChapterDetails {
    public class ChapterDetailsViewModel {
        public ChapterTypeVersionsEnum? VersionType { get; set; }

        public string RemoveSubChapterIds { get; set; }

        public int Id { get; set; }

        public IEnumerable<int> ProducedByIds { get; set; }

        public int IdChapter { get; set; }

        [Required]
        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ApprovementDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreateDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime UpdateDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ReviewDate { get; set; }

        public ChapterDetailsUserInfo AprovedByNavigation { get; set; }

        public ChapterDetailsUserInfo CreatedByNavigation { get; set; }

        public ChapterDetailsUserInfo IdReviewerNavigation { get; set; }

        public int? IdReviewer { get; set; }

        public int? IdApprover { get; set; }

        public List<UserChapterDetails> ProducedBy { get; set; }

        public int VersionNumber { get; set; }

        //public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        //public ChapterDetailsVersionInfo IdVersionInfoNavigation { get; set; }

        public ICollection<ChapterDetailsSubChapterVersion> SubChapterVersion { get; set; }

        public string WorkDetails { get; set; }

        public string WorkOrganization { get; set; }

        public string MachineTool { get; set; }

        public string AssociatedDetails { get; set; }

        public string WordDescription { get; set; }

        //Chapter on original
        public ChapterDetailsChapter IdChapterNavigation { get; set; }
    }
}
