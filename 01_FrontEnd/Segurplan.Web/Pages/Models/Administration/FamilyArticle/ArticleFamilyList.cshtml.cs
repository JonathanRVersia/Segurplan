using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration.Family;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Helpers;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.Web.Pages.Components.Warning;

namespace Segurplan.Web
{
    public class ArticleFamilyListModel : PageModel
    {
        #region props
        public IMediator mediator;

        public ILogger<ArticleFamilyListModel> logger;

        public UserManager<User> userManager;

        public FamilyListTableState TableState { get; set; } = new FamilyListTableState();
        public FamilyFilter TableFilter { get; set; } = new FamilyFilter();

        public List<int> PageRowList => TableState.PageRowList;

        public int TotalRows { get; set; }

        public int TotalPages { get; set; }

        public List<ApplicationFamily> FamilyList { get; set; }

        public enum FilterIndex {
            Id = 1,
            Family = 2
        }
        #endregion

        #region sort
        [BindProperty(SupportsGet = true)]
        public string IdSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FamilySort { get; set; }
        #endregion

        #region Binded props
        [BindProperty(SupportsGet = true)]
        [RegularExpression(@"\d*")]
        public string IdFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FamilyFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageRows { get; set; }

        [BindProperty]
        public string CurrentOrder { get; set; }
        #endregion

        public WarningDTO WarningDTO = new WarningDTO { Msg = "Family.Error.Delete" };

        public bool DeleteErrors { get; set; }

        #region Contructors
        public ArticleFamilyListModel(IMediator mediator, ILogger<ArticleFamilyListModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;

        }
        #endregion

        #region HTTP requests

        public virtual async Task<IActionResult> OnGetAsync(bool deleteErrors = false) {
            DeleteErrors = deleteErrors;

            if (TableState == null) {
                TableState = new FamilyListTableState();
            };
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }

        public virtual async Task OnGetApplySortAsync(string id, string family, int rows, string sortOrder) {
            CurrentOrder = sortOrder;
            await ApplySortAsync(id, family, rows, sortOrder);
        }

        public async Task<IActionResult> OnPostNewFamily() {
            return new LocalRedirectResult("/ArticleFamilyManagement?handler=CreateFamily");
        }

        public async Task<IActionResult> OnPost() {
            return await OnPostDeleteFilter(0);
        }

        public async Task<IActionResult> OnPostDeleteFilter(FilterIndex filterIndex) {
            switch (filterIndex) {
                case FilterIndex.Id:
                    IdFilter = string.Empty;
                    break;
                case FilterIndex.Family:
                    FamilyFilter = string.Empty;
                    break;
            }
            return await LoadInitList();
        }

        public virtual async Task OnPostApplySortAsync(string id, string family, int rows, string sortOrder) =>
            await ApplySortAsync(id, family, rows, sortOrder);

        public async Task<IActionResult> OnPostNextPage(int index) {
            TableState.IndexPage = index + 1;
            SetListOrder();
            return await ReloadList();
        }

        public async Task<IActionResult> OnPostPreviousPage(int index) {
            if (index > 0) {
                TableState.IndexPage = index - 1;
            }
            SetListOrder();
            return await ReloadList();
        }

        #endregion
        private async Task<IActionResult> LoadInitList() {
            CurrentOrder = string.Empty;
            SetListOrder();
            return await ReloadList().ConfigureAwait(true);
        }
        private async Task<IActionResult> ReloadList() {
            TableState.PageRows = PageRows;
            return await UpdateTable().ConfigureAwait(true);
        }
        private async Task<IActionResult> ApplySortAsync(string id, string family, int rows, string sortOrder) {

            if (TableState == null) {
                TableState = new FamilyListTableState();
            }
            TableState.PageRows = rows;
            FamilyFilter = family;
            IdFilter = id;
            CurrentOrder = sortOrder;
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }
        private async Task<IActionResult> UpdateTable() {
            FamilyFilter userFilters = new FamilyFilter {
                Family = FamilyFilter,
                Id = IdFilter
            };

            var responseList = await mediator.Send(new FamilyListRequest(TableState, userFilters)).ConfigureAwait(true);
            FamilyList = responseList.Value.FamilyList;
            TotalRows = responseList.Value.TotalRows;
            var pages = TotalRows / TableState.PageRows;
            if (pages > 0) {
                TotalPages = TotalRows % TableState.PageRows > 0
                    ? pages + 1
                    : pages;
            }
            return Page();
        }
        private void SetListOrder() {
            if (!string.IsNullOrEmpty(CurrentOrder)) {
                if (CurrentOrder.Contains(FamilyListTableState.IdFilter)) {
                    ApplyOrder(CurrentOrder, FamilyListTableState.IdFilter);
                } else if (CurrentOrder.Contains(FamilyListTableState.FamilyFilter)) {
                    ApplyOrder(CurrentOrder, FamilyListTableState.FamilyFilter);
                }
            } else {
                CurrentOrder = $"{FamilyListTableState.IdFilter}_Asc";
                TableState.OrderBy = FamilyListTableState.IdFilter;
                TableState.OrderModeDesc = ListOrderMode.Desc;
            }
            IdSort = SetOrder(CurrentOrder, FamilyListTableState.IdFilter);
            FamilySort = SetOrder(CurrentOrder, FamilyListTableState.FamilyFilter);
        }
        private void ApplyOrder(string sortOrder, string listColumnName) {
            TableState.OrderBy = listColumnName;
            TableState.OrderModeDesc = sortOrder == $"{listColumnName}_Asc" ? ListOrderMode.Desc : ListOrderMode.Asc;
        }
        private string SetOrder(string sortOrder, string listColumnName) {
            return !string.IsNullOrWhiteSpace(sortOrder) && sortOrder.Contains(listColumnName)
                ? sortOrder == $"{listColumnName}_Asc"
                    ? $"{listColumnName}_Desc"
                    : $"{listColumnName}_Asc"
                : $"{listColumnName}";
        }
    }
}
