using System.ComponentModel.DataAnnotations;
using Segurplan.Core.Actions.Administration.ChapterActivityList.Specifications;

namespace Segurplan.Web.Pages.Components.ChapterAndActivityList {
    public class ChapterAndActivityListSearch {
        [Display(Name = "ChaptersList.Search.ChapterId")]
        public int? ChapterNumber { get; set; }

        [Display(Name = "ChaptersList.Search.Title")]
        public string Title { get; set; }

        [Display(Name = "ChaptersList.Search.CreatedBy")]
        public string CreatedBy { get; set; }

        [Display(Name = "ChaptersList.Search.ReviewedBy")]
        public string ReviewedBy { get; set; }

        [Display(Name = "ChaptersList.Search.ApprovedBy")]
        public string ApprovedBy { get; set; }

        public FilterChapterActivityListSpecification ToSpecification() {
            var searchFilter = new FilterChapterActivityListSpecification();

            if (this.ChapterNumber != null)
                searchFilter.ByChapterNumber(this.ChapterNumber);
            if (this.Title != null)
                searchFilter.ByTitle(this.Title);
            if (this.CreatedBy != null)
                searchFilter.ByCreatedBy(this.CreatedBy);
            if (this.ReviewedBy != null)
                searchFilter.ByReviewedBy(this.ReviewedBy);
            if (this.ApprovedBy != null)
                searchFilter.ByApprovedBy(this.ApprovedBy);

            return searchFilter;
        }
    }
}
