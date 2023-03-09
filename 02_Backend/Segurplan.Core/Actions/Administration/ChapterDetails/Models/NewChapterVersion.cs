using System;
using System.Collections.Generic;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Models {
    public class NewChapterVersion {
        public int Id { get; set; }

        public int IdChapter { get; set; }

        public int VersionNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

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

        public List<UserChapterVersion> ProducedBy { get; set; }

        public Chapter IdChapterNavigation { get; set; }

        public ICollection<NewSubChapterVersion> SubChapterVersion { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int CreatedBy { get; set; }

        public int ModifiedBy { get; set; }
    }
}
