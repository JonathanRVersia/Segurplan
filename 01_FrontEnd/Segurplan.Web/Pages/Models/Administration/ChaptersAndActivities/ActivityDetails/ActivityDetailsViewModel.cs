using System;
using System.ComponentModel.DataAnnotations;
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;

namespace Segurplan.Web.Pages.Models.Administration.ChaptersAndActivities.ActivityDetails {
    public class ActivityDetailsViewModel {

        public int Id { get; set; }

        public int IdActivity { get; set; }

        public int IdSubChapterVersion { get; set; }

        public int Number { get; set; }

        [Required]
        public string Description { get; set; }

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

        public ActivityDetailsActivity IdActivityNavigation { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreateDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime UpdateDate { get; set; }

        //public ActivityDetailsSubChapterVersion IdSubChapterVersionNavigation { get; set; }

        public int? RelationsId { get; set; }
    }
}
