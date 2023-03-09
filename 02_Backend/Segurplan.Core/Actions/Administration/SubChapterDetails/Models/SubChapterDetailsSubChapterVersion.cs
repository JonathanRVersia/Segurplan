using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Models {
    public class SubChapterDetailsSubChapterVersion {
        public int Id { get; set; }

        public int IdSubChapter { get; set; }

        public int IdChapterVersion { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }//Puesto en campo Texto de la plantilla

        public string WorkDetails { get; set; }//Está en plantilla

        public string WorkOrganization { get; set; }//Está en plantilla

        //public string RiskEvaluation { get; set; }

        public string MachineTool { get; set; }//Está en plantilla

        public string AssociatedDetails { get; set; }

        //public string AssociatedDetails { get; set; }

        //public string SupportFacilities { get; set; }

        //public int? IdReviewer { get; set; }

        //public DateTime? ReviewDate { get; set; }

        //public int? IdApprover { get; set; }

        //public DateTime? ApprovementDate { get; set; }

        public SubChapterDetailsSubChapter IdSubChapterNavigation { get; set; }

        //public SubChapterDetailsChapter IdChapterVersionNavigation { get; set; }

        //public User AprovedByNavigation { get; set; }

        //public User IdReviewerNavigation { get; set; }

        //public User CreatedByNavigation { get; set; }

        //public User ModifiedByNavigation { get; set; }

        public ICollection<SubChapterDetailsActivity> ActivityVersion { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
