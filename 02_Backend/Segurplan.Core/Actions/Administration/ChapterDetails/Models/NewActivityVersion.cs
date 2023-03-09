using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Models {
    public class NewActivityVersion {

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

        public DateTime? ReviewDate { get; set; }

        public int? IdApprover { get; set; }

        public DateTime? ApprovementDate { get; set; }

        public NewActivity IdActivityNavigation { get; set; }

        public NewSubChapterVersion IdSubChapterVersionNavigation { get; set; }

        public ICollection<PlanActivityVersion> PlanActivityVersion { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int CreatedBy { get; set; }

        public int ModifiedBy { get; set; }
    }
}
