using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration.PreventiveMeasures;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.Core.Helpers;
using Segurplan.Web.Pages.Components.Warning;

namespace Segurplan.Web {
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class PreventiveMeasureListModel : PageModel {


        #region props
        public IMediator mediator;
        public ILogger<PreventiveMeasureListModel> logger;
        public UserManager<User> userManager;
        public MeasureListTableState TableState { get; set; } = new MeasureListTableState();
        public List<int> PageRowList => TableState.PageRowList;

        public int TotalRows { get; set; }
        public int TotalPages { get; set; }
        public List<ApplicationPreventiveMeasure> MeasureList { get; set; }
        public enum FilterIndex {
            Code = 1,
            Measure
        }
        #endregion
        #region sort
        [BindProperty(SupportsGet = true)]
        public string CodeSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MeasureSort { get; set; }
        #endregion
        #region Binded props
        [BindProperty(SupportsGet = true)]
        public string CodeFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MeasureFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageRows { get; set; }

        [BindProperty]
        public string CurrentOrder { get; set; }
        #endregion
        public WarningDTO WarningDTO = new WarningDTO { Msg = "Measures.Error.Delete" };
        public bool DeleteErrors { get; set; }

        #region Contructors
        public PreventiveMeasureListModel(IMediator mediator, ILogger<PreventiveMeasureListModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;

        }
        #endregion

        #region HTTP requests

        public virtual async Task<IActionResult> OnGetAsync(bool deleteErrors = false) {
            DeleteErrors = deleteErrors;
            if (TableState == null) {
                TableState = new MeasureListTableState();
            };
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }

        public virtual async Task OnGetApplySortAsync(string code, string measure, int rows, string sortOrder) {
            CurrentOrder = sortOrder;
            await ApplySortAsync(code, measure, rows, sortOrder);
        }

        public async Task<IActionResult> OnPostNewMeasure() {
            return new LocalRedirectResult("/MeasureManagement?handler=CreateMeasure");
        }

        public async Task<IActionResult> OnPost() {
            return await OnPostDeleteFilter(0);
        }

        public async Task<IActionResult> OnPostDeleteFilter(FilterIndex filterIndex) {
            switch (filterIndex) {
                case FilterIndex.Code:
                    CodeFilter = string.Empty;
                    break;
                case FilterIndex.Measure:
                    MeasureFilter = string.Empty;
                    break;
            }
            return await LoadInitList();
        }

        public virtual async Task OnPostApplySortAsync(string code, string measure, int rows, string sortOrder) =>
            await ApplySortAsync(code, measure, rows, sortOrder);

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
        private async Task<IActionResult> ApplySortAsync(string code, string measure, int rows, string sortOrder) {

            if (TableState == null) {
                TableState = new MeasureListTableState();
            }
            TableState.PageRows = rows;
            CodeFilter = code;
            MeasureFilter = measure;
            CurrentOrder = sortOrder;
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }


        private async Task<IActionResult> UpdateTable() {
            PreventiveMeasuresFilter userFilters = new PreventiveMeasuresFilter {
                Code = CodeFilter,
                Measure = MeasureFilter
            };

            var responseList = await mediator.Send(new MeasureListRequest(TableState, userFilters)).ConfigureAwait(true);
            MeasureList = responseList.Value.MeasureList;
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
                if (CurrentOrder.Contains(MeasureListTableState.CodeFilter)) {
                    ApplyOrder(CurrentOrder, MeasureListTableState.CodeFilter);
                } else if (CurrentOrder.Contains(MeasureListTableState.MeasureFilter)) {
                    ApplyOrder(CurrentOrder, MeasureListTableState.MeasureFilter);
                }
            } else {
                // Setting the default order
                CurrentOrder = $"{MeasureListTableState.CodeFilter}_Asc";
                TableState.OrderBy = MeasureListTableState.CodeFilter;
                TableState.OrderModeDesc = ListOrderMode.Desc;
            }
            CodeSort = SetOrder(CurrentOrder, MeasureListTableState.CodeFilter);
            MeasureSort = SetOrder(CurrentOrder, MeasureListTableState.MeasureFilter);
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
