using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Segurplan.Core.Actions.Administration.ChapterActivityList;
using Segurplan.Core.Actions.Administration.ChapterActivityList.Specifications;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Web.Pages.Components.ChapterAndActivityList {
    [Authorize]
    public class ChapterAndActivityListViewComponent : ViewComponent {

        private readonly IMediator mediator;

        public ChapterAndActivityListViewComponent(IMediator mediator) {
            this.mediator = mediator;
        }

        [BindProperty]
        public ChapterAndActivityListViewModel ChaptersModel { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(ChapterAndActivityListViewModel chaptersModel) {
            if (chaptersModel == null)
                chaptersModel = new ChapterAndActivityListViewModel();

            ChaptersModel = chaptersModel;

            ChaptersModel.ChaptersResponse = await mediator.Send(new ChapterActivityListRequest { Specifications = GetSpecifications() }).ConfigureAwait(true);

            SetPaginationValues();

            return View(ChaptersModel);
        }

        private IEnumerable<ISpecification<ChapterActivityListResponse.ListItem>> GetSpecifications() {
            var specifications = new List<ISpecification<ChapterActivityListResponse.ListItem>>();

            //specifications.Add(ByTypo());
            if (ChaptersModel.Search != null)
                specifications.Add(ChaptersModel.Search.ToSpecification());

            if (ChaptersModel.OrderBy?.SortOrder != null)
                specifications.Add(ChaptersModel.OrderBy.ToSpecification());
            else
                specifications.Add(ChaptersModel.OrderBy.DefaultSpecification());

            specifications.Add(GetPagination());

            return specifications;
        }

        public PaginatedChapterActivityListSpecification GetPagination() {
            var paginated = new PaginatedChapterActivityListSpecification(ChaptersModel.CurrentPage, ChaptersModel.PageSize);

            return paginated;
        }

        private void SetPaginationValues() {
            ChaptersModel.TotalPages = (int)Math.Ceiling(Decimal.Divide(ChaptersModel.ChaptersResponse?.Value?.TotalCount ?? 0, ChaptersModel.ChaptersResponse?.Value?.PageSize ?? 1));
            ChaptersModel.ShowPrevious = ChaptersModel.ChaptersResponse?.Value?.Page > 1;
            ChaptersModel.ShowNext = ChaptersModel.ChaptersResponse?.Value?.Page < ChaptersModel.TotalPages;
            ChaptersModel.ShowFirst = ChaptersModel.ChaptersResponse?.Value?.Page != 1 && ChaptersModel.TotalPages != 0;
            ChaptersModel.ShowLast = ChaptersModel.ChaptersResponse?.Value?.Page != ChaptersModel.TotalPages;
        }
    }
}
