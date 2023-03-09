using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration.Seriousness;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.Core.Helpers;
using Segurplan.Web.Pages.Components.Warning;
using Segurplan.Core.Actions.Administration;

namespace Segurplan.Web {
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class SeriousnessListModel : PageModel {

        #region props
        public IMediator mediator;

        public ILogger<SeriousnessListModel> logger;

        public UserManager<User> userManager;

        public SeriousnessListTableState TableState { get; set; } = new SeriousnessListTableState();

        public List<int> PageRowList => TableState.PageRowList;

        public int TotalRows { get; set; }

        public int TotalPages { get; set; }

        public List<ApplicationSeriousness> SeriousnessList { get; set; }

        public enum FilterIndex {
            Id = 1,
            Value = 2
        }
        #endregion

        #region sort
        [BindProperty(SupportsGet = true)]
        public string IdSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ValueSort { get; set; }
        #endregion

        #region Binded props
        [BindProperty(SupportsGet = true)]
        public int IdFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ValueFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageRows { get; set; }

        [BindProperty]
        public string CurrentOrder { get; set; }
        #endregion

        public WarningDTO WarningDTO = new WarningDTO { Msg = "Seriousness.Error.Delete" };

        public bool DeleteErrors { get; set; }

        #region Contructors
        public SeriousnessListModel(IMediator mediator, ILogger<SeriousnessListModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;

        }
        #endregion

        #region HTTP requests

        public virtual async Task<IActionResult> OnGetAsync(bool deleteErrors = false) {
            DeleteErrors = deleteErrors;

            if (TableState == null) {
                TableState = new SeriousnessListTableState();
            };
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }

        public virtual async Task OnGetApplySortAsync(int id, string value, int rows, string sortOrder) {
            CurrentOrder = sortOrder;
            await ApplySortAsync(id, value, rows, sortOrder);
        }

        public async Task<IActionResult> OnPostNewSeriousness() {
            return new LocalRedirectResult("/SeriousnessManagement?handler=CreateSeriousness");
        }

        public async Task<IActionResult> OnPost() {
            return await OnPostDeleteFilter(0);
        }

        public async Task<IActionResult> OnPostDeleteFilter(FilterIndex filterIndex) {
            switch (filterIndex) {
                case FilterIndex.Id:
                    IdFilter = 0;
                    break;
                case FilterIndex.Value:
                    ValueFilter = string.Empty;
                    break;
            }
            return await LoadInitList();
        }

        public virtual async Task OnPostApplySortAsync(int id, string value, int rows, string sortOrder) =>
            await ApplySortAsync(id, value, rows, sortOrder);

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

        private async Task<IActionResult> ApplySortAsync(int id, string value, int rows, string sortOrder) {

            if (TableState == null) {
                TableState = new SeriousnessListTableState();
            }
            TableState.PageRows = rows;
            IdFilter = id;
            ValueFilter = value;
            CurrentOrder = sortOrder;
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }

        private async Task<IActionResult> UpdateTable() {
            SeriousnessFilter userFilters = new SeriousnessFilter {
                Id = IdFilter,
                Value = ValueFilter
            };

            var responseList = await mediator.Send(new SeriousnessListRequest(TableState, userFilters)).ConfigureAwait(true);
            SeriousnessList = responseList.Value.SeriousnessList;
            TotalRows = responseList.Value.TotalRows;
            var pages = TotalRows / TableState.PageRows;
            if (pages > 0) {
                TotalPages = TotalRows % TableState.PageRows > 0
                    ? pages + 1
                    : pages;
            }
            return Page();
        }

        private async Task<IActionResult> LoadInitList() {
            CurrentOrder = string.Empty;
            SetListOrder();
            return await ReloadList();
        }

        private async Task<IActionResult> ReloadList() {
            TableState.PageRows = PageRows;
            return await UpdateTable().ConfigureAwait(true);
        }

        private void SetListOrder() {
            if (!string.IsNullOrEmpty(CurrentOrder)) {
                if (CurrentOrder.Contains(SeriousnessListTableState.IdFilter)) {
                    ApplyOrder(CurrentOrder, SeriousnessListTableState.IdFilter);
                } else if (CurrentOrder.Contains(SeriousnessListTableState.ValueFilter)) {
                    ApplyOrder(CurrentOrder, SeriousnessListTableState.ValueFilter);
                }
            } else {
                // Setting the default order
                CurrentOrder = $"{SeriousnessListTableState.IdFilter}_Asc";
                TableState.OrderBy = SeriousnessListTableState.IdFilter;
                TableState.OrderModeDesc = ListOrderMode.Desc;
            }
            IdSort = SetOrder(CurrentOrder, SeriousnessListTableState.IdFilter);
            ValueSort = SetOrder(CurrentOrder, SeriousnessListTableState.ValueFilter);
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
