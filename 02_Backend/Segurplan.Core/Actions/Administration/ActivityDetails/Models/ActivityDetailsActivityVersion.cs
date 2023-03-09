using System;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Models {
    public class ActivityDetailsActivityVersion {
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

        public DateTime? ReviewDate { get; set; }

        public int? IdApprover { get; set; }

        public DateTime? ApprovementDate { get; set; }

        public ActivityDetailsActivity IdActivityNavigation { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int? RelationsId { get; set; }

        //public ActivityDetailsSubChapterVersion IdSubChapterVersionNavigation { get; set; }

        //public User AprovedByNavigation { get; set; }

        //public User IdReviewerNavigation { get; set; }

        //public User CreatedByNavigation { get; set; }

        //public User ModifiedByNavigation { get; set; }

        //public ICollection<PlanActivityVersion> PlanActivityVersion { get; set; }
    }
}
