using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration.Templates;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.Core.Helpers;

namespace Segurplan.Web {
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class TemplateListModel : PageModel {

        #region props
        public IMediator mediator;
        public ILogger<TemplateListModel> logger;
        public UserManager<User> userManager;
        public TemplateListTableState TableState { get; set; } = new TemplateListTableState();
        public List<int> PageRowList => TableState.PageRowList;
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }
        public List<ApplicationTemplate> TemplateList { get; set; }
        public enum FilterIndex {
            Id = 1,
            Name,
            Notes,
            CreatedBy,
            ModifiedDate
        }
        #endregion

        #region sort
        [BindProperty(SupportsGet = true)]
        public string IdSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string NameSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string NotesSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CreatedBySort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ModifiedDateSort { get; set; }
        #endregion

        #region Binded props
        [BindProperty(SupportsGet = true)]
        public string IdFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string NameFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string NotesFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CreatedByFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ModifiedDateFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageRows { get; set; }

        [BindProperty]
        public string CurrentOrder { get; set; }
        #endregion

        #region Contructors
        public TemplateListModel(IMediator mediator, ILogger<TemplateListModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;

        }
        #endregion

        #region HTTP requests
        public virtual async Task<IActionResult> OnGetAsync() {
            if (TableState == null) {
                TableState = new TemplateListTableState();
            };
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }

        public virtual async Task OnGetApplySortAsync(string id, string template, string notes, string createdBy, string modifiedDate,  int rows, string sortOrder) {
            CurrentOrder = sortOrder;
            await ApplySortAsync(id, template, notes, createdBy, modifiedDate, rows, sortOrder);
        }

        public async Task<IActionResult> OnPostNewTemplate() {
            return new LocalRedirectResult("/TemplateManagement?handler=CreateTemplate");
        }

        public async Task<IActionResult> OnPost() {
            return await OnPostDeleteFilter(0);
        }

        public async Task<IActionResult> OnPostDeleteFilter(FilterIndex filterIndex) {
            switch (filterIndex) {
                case FilterIndex.Id:
                    IdFilter = string.Empty;
                    break;
                case FilterIndex.Name:
                    NameFilter = string.Empty;
                    break;
                case FilterIndex.Notes:
                    NotesFilter = string.Empty;
                    break;
                case FilterIndex.CreatedBy:
                    CreatedByFilter = string.Empty;
                    break;
                case FilterIndex.ModifiedDate:
                    ModifiedDateFilter = string.Empty;
                    break;
            }
            return await LoadInitList();
        }

        public virtual async Task OnPostApplySortAsync(string id, string template, string notes, string createdBy, string modifiedDate, int rows, string sortOrder) => 
            await ApplySortAsync(id, template, notes, createdBy, modifiedDate, rows, sortOrder);

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
        private async Task<IActionResult> ApplySortAsync(string id, string template, string notes, string createdBy, string modifiedDate, int rows, string sortOrder) {
            if (TableState == null) {
                TableState = new TemplateListTableState();
            }
            TableState.PageRows = rows;
            IdFilter = id;
            NameFilter = template;
            NotesFilter = notes;
            CreatedByFilter = createdBy;
            ModifiedDateFilter = modifiedDate;
            CurrentOrder = sortOrder;
            SetListOrder();
            return await UpdateTable().ConfigureAwait(true);
        }

        private async Task<IActionResult> UpdateTable() {
            TemplatesFilter userFilters = new TemplatesFilter {
                Id = IdFilter,
                Name = NameFilter,
                Notes = NotesFilter,
                CreatedBy = CreatedByFilter,
                ModifiedDate = ModifiedDateFilter
            };

            var responseList = await mediator.Send(new TemplateListRequest(TableState, userFilters)).ConfigureAwait(true);
            TemplateList = responseList.Value.TemplateList;
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
                if (CurrentOrder.Contains(TemplateListTableState.IdFilter)) {
                    ApplyOrder(CurrentOrder, TemplateListTableState.IdFilter);
                } else if (CurrentOrder.Contains(TemplateListTableState.NameFilter)) {
                    ApplyOrder(CurrentOrder, TemplateListTableState.NameFilter);
                } else if (CurrentOrder.Contains(TemplateListTableState.NotesFilter)) {
                    ApplyOrder(CurrentOrder, TemplateListTableState.NotesFilter);
                } else if (CurrentOrder.Contains(TemplateListTableState.CreatedByFilter)) {
                    ApplyOrder(CurrentOrder, TemplateListTableState.CreatedByFilter);
                } else if (CurrentOrder.Contains(TemplateListTableState.ModifiedDateFilter)) {
                    ApplyOrder(CurrentOrder, TemplateListTableState.ModifiedDateFilter);
                }
            } else {
                // Setting the default order
                CurrentOrder = $"{TemplateListTableState.IdFilter}_Asc";
                TableState.OrderBy = TemplateListTableState.IdFilter;
                TableState.OrderModeDesc = ListOrderMode.Desc;
                }
                IdSort = SetOrder(CurrentOrder, TemplateListTableState.IdFilter);
                NameSort = SetOrder(CurrentOrder, TemplateListTableState.NameFilter);
                NotesSort = SetOrder(CurrentOrder, TemplateListTableState.NotesFilter);
                CreatedBySort = SetOrder(CurrentOrder, TemplateListTableState.CreatedByFilter);
                ModifiedDateSort = SetOrder(CurrentOrder, TemplateListTableState.ModifiedDateFilter);
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
