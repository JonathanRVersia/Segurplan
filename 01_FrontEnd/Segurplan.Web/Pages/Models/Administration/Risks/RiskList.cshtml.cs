using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration.Risks;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.Core.Helpers;
using Segurplan.Web.Pages.Components.Warning;
using Segurplan.Core.Actions.Administration;


namespace Segurplan.Web 
{
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class RiskListModel : PageModel
    {
        #region props
        public IMediator mediator;

        public ILogger<RiskListModel> logger;

        public UserManager<User> userManager;

        public RiskListTableState TableState { get; set; } = new RiskListTableState();
        public RiskFilter TableFilter { get; set; } = new RiskFilter();

        public List<int> PageRowList => TableState.PageRowList;

        public int TotalRows { get; set; }

        public int TotalPages { get; set; }

        public List<ApplicationRisk> RiskList { get; set; }

        public enum FilterIndex {
            Id = 1,
            Name = 2,
            Code = 3
        }
        #endregion

        #region sort
        [BindProperty(SupportsGet = true)]
        public string IdSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string NameSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CodeSort { get; set; }
        #endregion

        #region Binded props
        [BindProperty(SupportsGet = true)]
        public int IdFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string NameFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CodeFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageRows { get; set; }

        [BindProperty]
        public string CurrentOrder { get; set; }
        #endregion

        public WarningDTO WarningDTO = new WarningDTO { Msg = "Risk.Error.Delete" };

        public bool DeleteErrors { get; set; }

        #region Contructors
        public RiskListModel(IMediator mediator, ILogger<RiskListModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;

        }
        #endregion

        #region HTTP requests

        public virtual async Task<IActionResult> OnGetAsync(bool deleteErrors = false) {
            DeleteErrors = deleteErrors;

            if (TableState == null) {
                TableState = new RiskListTableState();
            };
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }

        public virtual async Task OnGetApplySortAsync(int id,string code, string name, int rows, string sortOrder) {
            CurrentOrder = sortOrder;
            await ApplySortAsync(id,code, name, rows, sortOrder);
        }

        public async Task<IActionResult> OnPostNewRisk() {
            return new LocalRedirectResult("/RiskManagement?handler=CreateRisk");
        }

        public async Task<IActionResult> OnPost() {
            return await OnPostDeleteFilter(0);
        }

        public async Task<IActionResult> OnPostDeleteFilter(FilterIndex filterIndex) {
            switch (filterIndex) {
                case FilterIndex.Id:
                    IdFilter = 0;
                    break;
                case FilterIndex.Name:
                    NameFilter = string.Empty;
                    break;
                case FilterIndex.Code:
                    CodeFilter = string.Empty;
                    break;
            }
            return await LoadInitList();
        }

        public virtual async Task OnPostApplySortAsync(int id,string code, string name, int rows, string sortOrder) =>
            await ApplySortAsync(id,code, name, rows, sortOrder);

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

        private async Task<IActionResult> ApplySortAsync(int id,string code, string name, int rows, string sortOrder) {

            if (TableState == null) {
                TableState = new RiskListTableState();
            }
            TableState.PageRows = rows;
            IdFilter = id;
            NameFilter = name;
            CodeFilter = code;
            CurrentOrder = sortOrder;
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }

        private async Task<IActionResult> UpdateTable() {
            RiskFilter userFilters = new RiskFilter {
                Id = IdFilter,
                Code = CodeFilter,
                Name = NameFilter
            };

            var responseList = await mediator.Send(new RiskListRequest(TableState, userFilters)).ConfigureAwait(true);
            RiskList = responseList.Value.RiskList;
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
                if (CurrentOrder.Contains(RiskListTableState.IdFilter)) {
                    ApplyOrder(CurrentOrder, RiskListTableState.IdFilter);
                } else if (CurrentOrder.Contains(RiskListTableState.NameFilter)) {
                    ApplyOrder(CurrentOrder, RiskListTableState.NameFilter);
                } else if (CurrentOrder.Contains(RiskListTableState.CodeFilter)) {
                    ApplyOrder(CurrentOrder, RiskListTableState.CodeFilter);
                }
        } else {
                // Setting the default order
                CurrentOrder = $"{RiskListTableState.IdFilter}_Asc";
                TableState.OrderBy = RiskListTableState.IdFilter;
                TableState.OrderModeDesc = ListOrderMode.Desc;
            }
            IdSort = SetOrder(CurrentOrder, RiskListTableState.IdFilter);
            NameSort = SetOrder(CurrentOrder, RiskListTableState.NameFilter);
            CodeSort = SetOrder(CurrentOrder, RiskListTableState.CodeFilter);
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
