using Segurplan.Core.Actions.Administration.ChapterActivityList;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Web.Pages.Components.ChapterAndActivityList {
    public class ChapterAndActivityListViewModel {
        public int TotalPages;
        public bool ShowPrevious;
        public bool ShowNext;
        public bool ShowFirst;
        public bool ShowLast;

        public int PageSize { get; set; } = 100;

        public int CurrentPage { get; set; }

        public ChapterAndActivityListOrderBy OrderBy { get; set; } = new ChapterAndActivityListOrderBy();

        public ChapterAndActivityListSearch Search { get; set; }

        public IRequestResponse<ChapterActivityListResponse> ChaptersResponse { get; set; }
        public int ChapterToRemoveId { get; set; }
    }
}
