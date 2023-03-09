using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration.Tasks;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Helpers;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.Web.Pages.Components.Warning;

namespace Segurplan.Web {
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class TaskArticleListModel : PageModel {
        #region props
        IMediator mediator;
        ILogger<SeriousnessListModel> logger;
        UserManager<User> userManager;
        public TasksListTableState TableState { get; set; } = new TasksListTableState();
        public bool DeleteErrors { get; set; }
        public List<ApplicationTask> TasksList { get; set; }
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }
        public enum FilterIndex {
            Name = 1
        }
        #endregion
        #region Binded props
        [BindProperty(SupportsGet = true)]
        public string NameFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FamilyFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageRows { get; set; }

        [BindProperty]
        public string CurrentOrder { get; set; }
        public WarningDTO WarningDTO = new WarningDTO { Msg = "Task.Error.Delete" };
        #endregion
        #region sort
        [BindProperty(SupportsGet = true)]
        public string IdSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string NameSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PercentageSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TimeOfWorkSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string AmortizationTimeSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MinimumUnitSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PriceSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FamilySort { get; set; }
        #endregion
        public TaskArticleListModel(IMediator mediator, ILogger<SeriousnessListModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;
        }

        #region HTTP request
        public virtual async Task<IActionResult> OnGetAsync(bool deleteErrors = false) {
            DeleteErrors = deleteErrors;

            if (TableState == null) {
                TableState = new TasksListTableState();
            };
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }
        public async Task<IActionResult> OnPostDeleteFilter(FilterIndex filterIndex) {
            switch (filterIndex) {
                case FilterIndex.Name:
                    NameFilter = string.Empty;
                    break;
            }
            return await LoadInitList().ConfigureAwait(true);
        }
        private async Task<IActionResult> LoadInitList() {
            CurrentOrder = string.Empty;
            SetListOrder();
            return await ReloadList().ConfigureAwait(true);
        }
        private async Task<IActionResult> ReloadList() {
            TableState.PageRows = PageRows;
            return await UpdateTable().ConfigureAwait(true);
        }
        public virtual async Task OnPostApplySortAsync(string family, string name, int rows, string sortOrder) =>
            await ApplySortAsync(family, name, rows, sortOrder);

        public virtual async Task OnGetApplySortAsync(string family, string name, int rows, string sortOrder) {
            CurrentOrder = sortOrder;
            await ApplySortAsync(family, name, rows, sortOrder);
        }
        #endregion
        private async Task<IActionResult> ApplySortAsync(string family, string name, int rows, string sortOrder) {

            if (TableState == null) {
                TableState = new TasksListTableState();
            }
            TableState.PageRows = rows;
            FamilyFilter = family;
            NameFilter = name;
            CurrentOrder = sortOrder;
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }
        private async Task<IActionResult> UpdateTable() {
            TasksFilter userFilters = new TasksFilter {
                Name = NameFilter,
            };

            var responseList = await mediator.Send(new TasksListRequest(TableState, userFilters)).ConfigureAwait(true);
            TasksList = responseList.Value.TasksList;
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
                if (CurrentOrder.Contains(TasksListTableState.IdSort)) {
                    ApplyOrder(CurrentOrder, TasksListTableState.IdSort);
                } else if (CurrentOrder.Contains(TasksListTableState.NameSort)) {
                    ApplyOrder(CurrentOrder, TasksListTableState.NameSort);
                }
            } else {
                CurrentOrder = $"{TasksListTableState.IdSort}_Asc";
                TableState.OrderBy = TasksListTableState.IdSort;
                TableState.OrderModeDesc = ListOrderMode.Desc;
            }
            IdSort = SetOrder(CurrentOrder, TasksListTableState.IdSort);
            NameSort = SetOrder(CurrentOrder, TasksListTableState.NameSort);
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
        public async Task<IActionResult> OnPostNextPage(int index) {
            TableState.IndexPage = index + 1;
            SetListOrder();
            return await ReloadList().ConfigureAwait(true);
        }
        public async Task<IActionResult> OnPostPreviousPage(int index) {
            if (index > 0) {
                TableState.IndexPage = index - 1;
            }
            SetListOrder();
            return await ReloadList().ConfigureAwait(true);
        }
        public async Task<IActionResult> OnPostNewTask() {
            return new LocalRedirectResult("/TaskManagement?handler=CreateTask");
        }
    }
}
