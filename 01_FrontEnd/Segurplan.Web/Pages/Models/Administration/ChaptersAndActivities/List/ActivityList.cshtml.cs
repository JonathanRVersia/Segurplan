using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Segurplan.Core.Actions.Administration.ChapterActivityList;
using Segurplan.Core.Actions.Administration.ChapterActivityList.Export;
using Segurplan.Core.Actions.Administration.ChapterDetails.Delete;
using Segurplan.Core.Actions.Administration.ChapterDetails.DeleteChapterVersionCheck;
using Segurplan.Core.Actions.Administration.ChapterDetails.DeleteCheck;
using Segurplan.Core.Actions.Administration.ChapterDetails.DeleteVersion;
using Segurplan.Core.Actions.Administration.ChapterDetails.GetChapterVersions;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.Query;
using Segurplan.Web.Pages.Components.ChapterActivitiesDeleteCheckModal;
using Segurplan.Web.Pages.Components.ChapterAndActivityList;
using Segurplan.Web.Pages.Components.ChapterVersionModalTable;

namespace Segurplan.Web.Pages.Models.Administration.ChaptersAndActivities.List {
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class ActivityListModel : PageModel {

        private readonly IMediator mediator;

        public ActivityListModel(IMediator mediator) {
            this.mediator = mediator;
        }

        [BindProperty]
        public ChapterAndActivityListViewModel ChaptersModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ChapterToRemoveId { get; set; }

        //[BindProperty]
        //public int SubChapterToRemoveId { get; set; }

        //[BindProperty]
        //public int ActivityToRemoveId { get; set; }

        //User for check if Chapter/SubChapter can be deleted. If not null in view shows modal
        public ChapterActivitiesDeleteCheckModalModel DeleteCheck { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            await RemoveChapterSuchapterAndActivity().ConfigureAwait(true);

            return Page();
        }

        public async Task<IActionResult> OnPost(string sortOrder, int currentPage) {

            await RemoveChapterSuchapterAndActivity().ConfigureAwait(true);

            if (ChaptersModel.OrderBy == null)
                ChaptersModel.OrderBy = new ChapterAndActivityListOrderBy();

            ChaptersModel.OrderBy.SortOrder = sortOrder;
            ChaptersModel.CurrentPage = currentPage;

            return Page();
        }

        private async Task RemoveChapterSuchapterAndActivity() {
            //if (ActivityToRemoveId != 0) {
            //    var checkDeleteActivityResponse = await mediator.Send(new DeleteCheckActivityRequest { ActivityId = ActivityToRemoveId }).ConfigureAwait(true);
            //    if (checkDeleteActivityResponse.Status == RequestStatus.NoContent)
            //        await mediator.Send(new DeleteActivityRequest { ActivityId = ActivityToRemoveId }).ConfigureAwait(true);
            //    else
            //        DeleteCheck = new ChapterActivitiesDeleteCheckModalModel {
            //            ActivityHasPlan = checkDeleteActivityResponse.Value.ActivityHasPlan,
            //        };
            //}

            //if (SubChapterToRemoveId != 0) {
            //    var checkDeleteSubChapterResponse = await mediator.Send(new DeleteCheckSubChapterRequest { SubChapterId = SubChapterToRemoveId }).ConfigureAwait(true);
            //    if (checkDeleteSubChapterResponse.Status == RequestStatus.NoContent)
            //        await mediator.Send(new DeleteSubChapterRequest { SubChapterId = SubChapterToRemoveId }).ConfigureAwait(true);
            //    else
            //        DeleteCheck = new ChapterActivitiesDeleteCheckModalModel {
            //            ActivityHasPlan = checkDeleteSubChapterResponse.Value.ActivityHasPlan,
            //            SubChapterToRemoveIds = checkDeleteSubChapterResponse.Value.RiskPreventiveSubChapterIds
            //        };
            //}

            if (ChapterToRemoveId != 0) {
                var checkDeleteChapterResponse = await mediator.Send(new DeleteCheckChapterRequest { ChapterId = ChapterToRemoveId }).ConfigureAwait(true);
                if (checkDeleteChapterResponse.Status == RequestStatus.NoContent)
                    await mediator.Send(new DeleteChapterRequest { ChapterId = ChapterToRemoveId }).ConfigureAwait(true);
                else
                    DeleteCheck = new ChapterActivitiesDeleteCheckModalModel {
                        ActivityHasPlan = checkDeleteChapterResponse.Value.ActivityHasPlansOrPreventiveMeasures,
                        SubChapterToRemoveIds = checkDeleteChapterResponse.Value.SubChapterRiskPreventiveIds,
                        ChapterToRemoveId = checkDeleteChapterResponse.Value.ChapterRiskPreventiveId
                    };
            }
        }

        public async Task<IActionResult> OnPostDeleteChapterVersionAsync(int versionId, int chapterId) {

            var checkResponse = await mediator.Send(new DeleteChapterVersionCheckRequest { ChapterVersionId = versionId }).ConfigureAwait(true);

            if (checkResponse.Status != RequestStatus.NoContent) {
                DeleteCheck = new ChapterActivitiesDeleteCheckModalModel {
                    ActivityHasPlan = checkResponse.Value.ActivityHasPlansOrPreventiveMeasures,
                    SubChapterToRemoveIds = checkResponse.Value.SubChapterRiskPreventiveIds,
                    ChapterToRemoveId = checkResponse.Value.ChapterRiskPreventiveId
                };
            } else {
                var deleteResponse = await mediator.Send(new DeleteVersionRequest { VersionId = versionId }).ConfigureAwait(true);

                if (deleteResponse.Status != RequestStatus.Ok)
                    return null;
            }

            var getResponse = await mediator.Send(new GetChapterVersionsRequest { ChapterId = chapterId }).ConfigureAwait(true);

            var modalList = new ChapterVersionModalTableModel { ChapterVersions = getResponse.Value.ChapterVersions, HideDeleteIfOnlyOneVersion = true, DeleteCheck = this.DeleteCheck };

            return new ViewComponentResult() {
                ViewComponentName = "ChapterVersionModalTable",
                Arguments = new {
                    modalList
                },
                ViewData = this.ViewData,
                TempData = this.TempData
            };
        }

        public async Task<IActionResult> OnPostExportToWordAsync() {
            var specification = new List<ISpecification<ChapterActivityListResponse.ListItem>>();

            specification.Add(new ChapterAndActivityListOrderBy().DefaultSpecification());

            var chapterActivityList = await mediator.Send(new ChapterActivityListRequest { Specifications = specification }).ConfigureAwait(true);
            var file = await mediator.Send(new ChapterArtivityToWordRequest { Chapters = chapterActivityList.Value.Chapters }).ConfigureAwait(true);

            return File(file.Value.MemoryFile.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", DateTime.Now.ToString("yyyy/MM/dd") + "_ListadoCapitulos.docx");

        }
        //    #region props
        //    public IMediator mediator;
        //    public ILogger<ActivityListModel> logger;
        //    public UserManager<User> userManager;
        //    public ActivityListTableState TableState { get; set; } = new ActivityListTableState();
        //    public List<int> PageRowList {
        //        get { return TableState.PageRowList; }
        //    }

        //    public int TotalRows { get; private set; }
        //    public int TotalPages { get; private set; }
        //    [BindProperty]
        //    public List<ApplicationActivity> ActivityList { get; set; }
        //    public enum FilterIndex {
        //        Chapter = 1,
        //        Subchapter,
        //        Description,
        //        Number,
        //        Version
        //    }
        //    #endregion

        //    #region Binded props
        //    [BindProperty(SupportsGet = true)]
        //    public string ChapterFilter { get; set; }

        //    [BindProperty(SupportsGet = true)]
        //    public string SubchapterFilter { get; set; }

        //    [BindProperty(SupportsGet = true)]
        //    public string DescriptionFilter { get; set; }

        //    [BindProperty(SupportsGet = true)]
        //    public string NumberFilter { get; set; }

        //    [BindProperty(SupportsGet = true)]
        //    public string VersionFilter { get; set; }

        //    [BindProperty(SupportsGet = true)]
        //    public string ChapterSort { get; set; }

        //    [BindProperty(SupportsGet = true)]
        //    public string SubchapterSort { get; set; }

        //    [BindProperty(SupportsGet = true)]
        //    public string DescriptionSort { get; set; }

        //    [BindProperty(SupportsGet = true)]
        //    public string NumberSort { get; set; }

        //    [BindProperty(SupportsGet = true)]
        //    public string VersionSort { get; set; }

        //    [BindProperty]
        //    public string CurrentOrder { get; set; }

        //    [BindProperty(SupportsGet = true)]
        //    public int IndexPage { get; set; }

        //    [BindProperty(SupportsGet = true)]
        //    public int PageRows { get; set; }


        //    #endregion

        //    #region Contructors
        //    public ActivityListModel(IMediator mediator, ILogger<ActivityListModel> logger, UserManager<User> userManager) {
        //        this.mediator = mediator;
        //        this.logger = logger;
        //        this.userManager = userManager;

        //    }
        //    #endregion

        //    #region HTTP requests


        //    public async Task<IActionResult> OnPostNewMeasure() {
        //        return new LocalRedirectResult("/ActivityManagement?handler=CreateActivity");
        //    }
        //    public async Task<IActionResult> OnGetAsync() {
        //        if (TableState == null)
        //            TableState = new ActivityListTableState() {

        //            };
        //        SetListOrder();

        //        return await UpdateTable().ConfigureAwait(true);
        //    }
        //    public async Task<IActionResult> OnPost() {

        //        return await OnPostDeleteFilter(0); 
        //    }

        //    public async Task<IActionResult> OnPostDeleteFilter(FilterIndex filterIndex) {
        //        switch (filterIndex) {
        //            case FilterIndex.Chapter:
        //                ChapterFilter = string.Empty;
        //                break;
        //            case FilterIndex.Subchapter:
        //                SubchapterFilter = string.Empty;
        //                break;
        //            case FilterIndex.Description:
        //                DescriptionFilter = string.Empty;
        //                break;
        //            case FilterIndex.Number:
        //                NumberFilter = string.Empty;
        //                break;
        //            case FilterIndex.Version:
        //                VersionFilter = string.Empty;
        //                break;
        //        }
        //        return await LoadInitList();


        //    }

        //    public async Task<IActionResult> OnPostNextPage(int indexPage) {

        //        TableState.IndexPage = indexPage + 1;
        //        SetListOrder();
        //        return await ReloadList();
        //    }
        //    public async Task<IActionResult> OnPostPreviousPage(int indexPage) {

        //        if (indexPage > 0) {
        //            TableState.IndexPage = indexPage - 1;
        //        }
        //        SetListOrder();
        //        return await ReloadList();
        //    }
        //    public virtual async Task OnPostApplySortAsync(string chapter, string subChapter, string description, string number, string version, int rows, string sortOrder) =>
        //      await ApplySortAsync(chapter, subChapter, description, number, version, rows, sortOrder);

        //    public virtual async Task OnGetApplySortAsync(string chapter, string subChapter, string description, string number, string version, int rows, string sortOrder) {
        //        CurrentOrder = sortOrder;
        //        await ApplySortAsync(chapter, subChapter, description, number, version, rows, sortOrder);

        //    }


        //    #endregion

        //    private async Task<IActionResult> ApplySortAsync(string chapter, string subChapter, string description, string number, string version, int rows, string sortOrder) {

        //        if (TableState == null) {
        //            TableState = new ActivityListTableState();
        //        }
        //        TableState.PageRows = rows;
        //        ChapterFilter = chapter;
        //        SubchapterFilter = subChapter;
        //        DescriptionFilter = description;
        //        NumberFilter = number;
        //        VersionFilter = version;
        //        CurrentOrder = sortOrder;
        //        SetListOrder();
        //        return await UpdateTable().ConfigureAwait(true);
        //    }

        //    private async Task<IActionResult> UpdateTable() {


        //        ActivityFilter userFilters = new ActivityFilter {
        //            Chapter = ChapterFilter,
        //            Subchapter = SubchapterFilter,
        //            Description = DescriptionFilter,               
        //            Number = NumberFilter,
        //            Version = VersionFilter
        //        };

        //        var responseList = await mediator.Send(new ActivityListRequest(TableState, userFilters)).ConfigureAwait(true);
        //        ActivityList = responseList.Value.ActivityList;
        //        TotalRows = responseList.Value.TotalRows;
        //        var pages = TotalRows / TableState.PageRows;
        //        if (pages > 0) {
        //            TotalPages = TotalRows % TableState.PageRows > 0
        //                ? pages + 1
        //                : pages;
        //        }
        //        return Page();
        //    }
        //    private async Task<IActionResult> LoadInitList() {
        //        CurrentOrder = string.Empty;
        //        SetListOrder();
        //        return await ReloadList();
        //    }

        //    private async Task<IActionResult> ReloadList() {
        //        TableState.PageRows = PageRows;
        //        return await UpdateTable().ConfigureAwait(true);
        //    }

        //    private void SetListOrder() {
        //        if (!string.IsNullOrEmpty(CurrentOrder)) {
        //            if (CurrentOrder.Contains(ActivityListTableState.ChapterFilter)) {
        //                ApplyOrder(CurrentOrder, ActivityListTableState.ChapterFilter);
        //            } else if (CurrentOrder.Contains(ActivityListTableState.SubchapterFilter)) {
        //                ApplyOrder(CurrentOrder, ActivityListTableState.SubchapterFilter);
        //            } else if (CurrentOrder.Contains(ActivityListTableState.DescriptionFilter)) {
        //                ApplyOrder(CurrentOrder, ActivityListTableState.DescriptionFilter);
        //            } else if (CurrentOrder.Contains(ActivityListTableState.NumberFilter)) {
        //                ApplyOrder(CurrentOrder, ActivityListTableState.NumberFilter);
        //            } else if (CurrentOrder.Contains(ActivityListTableState.VersionFilter)) {
        //                ApplyOrder(CurrentOrder, ActivityListTableState.VersionFilter);
        //            }

        //        } else {
        //            // Setting the default order
        //            CurrentOrder = $"{ActivityListTableState.ChapterFilter}_Dsc";
        //            TableState.OrderBy = ActivityListTableState.ChapterFilter;
        //            TableState.OrderModeDesc = ListOrderMode.Asc;
        //        }
        //        ChapterSort = SetOrder(CurrentOrder, ActivityListTableState.ChapterFilter);
        //        SubchapterSort = SetOrder(CurrentOrder, ActivityListTableState.SubchapterFilter);
        //        DescriptionSort = SetOrder(CurrentOrder, ActivityListTableState.DescriptionFilter);
        //        NumberSort = SetOrder(CurrentOrder, ActivityListTableState.NumberFilter);
        //        VersionSort = SetOrder(CurrentOrder, ActivityListTableState.VersionFilter);
        //    }

        //    private void ApplyOrder(string sortOrder, string listColumnName) {
        //        TableState.OrderBy = listColumnName;
        //        TableState.OrderModeDesc = sortOrder == $"{listColumnName}_Asc" ? ListOrderMode.Desc : ListOrderMode.Asc;
        //    }

        //    private string SetOrder(string sortOrder, string listColumnName) {
        //        return !string.IsNullOrWhiteSpace(sortOrder) && sortOrder.Contains(listColumnName)
        //            ? sortOrder == $"{listColumnName}_Asc"
        //                ? $"{listColumnName}_Dsc"
        //                : $"{listColumnName}_Asc"
        //            : $"{listColumnName}";
        //    }
    }

}
