using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Plans;
using Segurplan.Core.Actions.Plans.PlansData;
using Segurplan.Core.Actions.Plans.PlanLists;
using Segurplan.Core.Actions.Plans.PlanManagement.Delete;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Helpers;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.Web.Utils;
using Segurplan.Core.Actions.Plans.PlanManagement.Generate.Plan;
using Microsoft.Net.Http.Headers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Web.Pages.Models.SafetyPlans {
    public class PlansBase : PageModel {

        public IMediator mediator;
        public ILogger<MyPlans> logger;
        public UserManager<User> userManager;

        [BindProperty]
        public List<SafetyPlan> ListaPlanes { get; set; }

        public int TotalRows { get; set; }

        public enum FilterIndex {
            Activity = 1,
            Title,
            ProducedBy,
            CheckedBy,
            ApprovedBy,
            Organization,
            FromDate,
            ToDate
        }

        #region Bind properties

        [BindProperty]
        public TableState TableState { get; set; }

        [BindProperty]
        public string SelectedPlan { get; set; }

        [HiddenInput]
        public string DuplicatePlanId { get; set; }

        [BindProperty]
        public string DuplicateName { get; set; }

        [HiddenInput]
        public string DeletedPlan { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageRows { get; set; }

        [BindProperty]
        public int TotalPages { get; set; }

        #endregion

        #region Sortings

        [BindProperty]
        public string OrganizationSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string TitleSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CustomerSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ActivitySort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ProducedBySort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ModifiedBySort { get; set; }

        #endregion

        #region Filters

        [BindProperty]
        public string ActivityFilter { get; set; }

        [BindProperty]
        public string TitleFilter { get; set; }

        [BindProperty]
        public string ProducedByFilter { get; set; }

        [BindProperty]
        public string CheckedByFilter { get; set; }

        [BindProperty]
        public string ApprovedByFilter { get; set; }

        [BindProperty]
        public string OrganizationFilter { get; set; }

        [BindProperty]
        public string FromDateFilter { get; set; }

        [BindProperty]
        public string ToDateFilter { get; set; }

        [BindProperty]
        public string CurrentOrder { get; set; }

        [BindProperty]
        public string PrevPage { get; set; }
        #endregion

        public PlansBase(IMediator mediator, ILogger<MyPlans> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;
        }

        #region Get methods

        public virtual async Task OnGetAsync() {

            if (TableState == null) {
                TableState = new TableState();
            }

            SetListOrder();

            // Checking whether we want all or just my plans
            TableState.AllPlans = Url.Action() == "/AllPlans";

            SessionHelper.RemoveActivities(HttpContext.Session);

            await UpdateTable().ConfigureAwait(true);
        }

        public virtual async Task OnGetApplySortAsync(string activity, string title, string producedBy, string checkedBy,
            string approvedBy, string organization, string fromDate, string toDate, int rows, string sortOrder) {

            CurrentOrder = sortOrder;

            await ApplySortAsync(activity, title, producedBy, checkedBy, approvedBy, organization, fromDate, toDate, rows, sortOrder);
        }

        #endregion

        #region Post methods

        public async Task<IActionResult> OnPost() {
            return await OnPostLoadPlans(0);
        }

        public async Task<IActionResult> OnPostLoadPlans(FilterIndex filterIndex) {
            switch (filterIndex) {
                case FilterIndex.Activity:
                    ActivityFilter = string.Empty;
                    break;
                case FilterIndex.Title:
                    TitleFilter = string.Empty;
                    break;
                case FilterIndex.ApprovedBy:
                    ApprovedByFilter = string.Empty;
                    break;
                case FilterIndex.CheckedBy:
                    CheckedByFilter = string.Empty;
                    break;
                case FilterIndex.Organization:
                    OrganizationFilter = string.Empty;
                    break;
                case FilterIndex.ProducedBy:
                    ProducedByFilter = string.Empty;
                    break;
                case FilterIndex.FromDate:
                    FromDateFilter = string.Empty;
                    break;
                case FilterIndex.ToDate:
                    ToDateFilter = string.Empty;
                    break;
            }

            TableState.FirstLoad = false;

            return await LoadInitList();
        }

        public virtual async Task OnPostApplySortAsync(string activity, string title, string producedBy, string checkedBy,
            string approvedBy, string organization, string fromDate, string toDate, int rows, string sortOrder) =>
            await ApplySortAsync(activity, title, producedBy, checkedBy, approvedBy, organization, fromDate, toDate, rows, sortOrder);


        public async Task<IActionResult> OnPostDuplicate(string duplicatePlanId) =>
            LocalRedirect($"/PlanManagement?Id={duplicatePlanId}&op={PlanActionType.Duplicate}&title={DuplicateName}");

        public async Task<IActionResult> OnPostDeletePlan(string deletedPlan) {

            int userId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            await mediator.Send(new DeletePlanRequest {
                PlanId = Convert.ToInt32(deletedPlan),
                UserId = userId
            }).ConfigureAwait(true);

            TableState.FirstLoad = false;

            SetListOrder();

            return await ReloadList();
        }

        public async Task<IActionResult> OnPostNextPage(int index) {

            TableState.IndexPage = index + 1;
            TableState.FirstLoad = false;

            SetListOrder();

            return await ReloadList();
        }

        public async Task<IActionResult> OnPostPreviousPage(int index) {

            TableState.IndexPage = index - 1;

            if (TableState.IndexPage >= 0) {
                TableState.FirstLoad = false;
            }

            SetListOrder();

            return await ReloadList();
        }

        public async Task<IActionResult> OnGetCreateDocument(int planID) {
            string templateName = "PLAN DE SEGURIDAD.docx";

            var response = await mediator.Send(new GeneratePlanRequest() { PlanId = planID, TemplateName = templateName }).ConfigureAwait(false);
            if (response.Status == RequestStatus.Ok) {
                return new FileStreamResult(response.Value.Document.ResponseStream, new MediaTypeHeaderValue(response.Value.Document.MediaType));
            } else {
                return new NoContentResult();
            }
        }

        #endregion

        #region Private Helpers

        private async Task<IActionResult> LoadInitList() {

            CurrentOrder = string.Empty;
            SetListOrder();

            return await ReloadList();
        }

        private async Task ApplySortAsync(string activity, string title, string producedBy, string checkedBy, string approvedBy,
            string organization, string fromDate, string toDate, int rows, string sortOrder) {

            if (TableState == null) {
                TableState = new TableState();
                TableState.FirstLoad = true;
            } else {
                TableState.FirstLoad = false;
            }

            TableState.PageRows = rows;

            ActivityFilter = activity;
            TitleFilter = title;
            ProducedByFilter = producedBy;
            CheckedByFilter = checkedBy;
            ApprovedByFilter = approvedBy;
            OrganizationFilter = organization;
            FromDateFilter = fromDate;
            ToDateFilter = toDate;

            CurrentOrder = sortOrder;
            SetListOrder();

            if (!string.IsNullOrEmpty(sortOrder)) {
                TableState.FirstLoad = false;
            }

            // Checking whether we want all or just my plans
            TableState.AllPlans = Url.Action() == "/AllPlans";

            await UpdateTable().ConfigureAwait(true);
        }

        private async Task<IActionResult> ReloadList() {
            // Checking whether we want all or just my plans
            TableState.AllPlans = Url.Action() == "/AllPlans";

            TableState.PageRows = PageRows;

            return await UpdateTable();
        }
        private async Task<IActionResult> UpdateTable() {

            Filter userFilters = new Filter {
                Activity = ActivityFilter,
                Title = TitleFilter,
                ApprovedBy = ApprovedByFilter,
                CheckedBy = CheckedByFilter,
                Organization = OrganizationFilter,
                ProducedBy = ProducedByFilter,
                ProducedFromDate = FromDateFilter,
                ProducedToDate = ToDateFilter
            };

            if (PageRows != 0 && TableState.PageRows != PageRows)
                TableState.PageRows = PageRows;


            var respuestaLista = TableState.AllPlans
                    ? await mediator.Send(new AllSafetyPlanRequest(TableState, userFilters, userManager.GetUserId(HttpContext.User))).ConfigureAwait(true)
                    : await mediator.Send(new MySafetyPlanRequest(TableState, userFilters, userManager.GetUserId(HttpContext.User))).ConfigureAwait(true);

            ListaPlanes = respuestaLista.Value.ListaPlanes;
            TotalRows = respuestaLista.Value.TotalRows;

            int pages = TotalRows / TableState.PageRows;

            if (pages > 0) {
                TotalPages = TotalRows % TableState.PageRows > 0
                    ? pages + 1
                    : pages;
            }

            RightsController.UserRoles = respuestaLista.Value.MyRoleIDs;

            return Page();
        }

        private void SetListOrder() {
            if (!string.IsNullOrEmpty(CurrentOrder)) {
                if (CurrentOrder.Contains(TableState.OrganizationFilter)) {
                    ApplyOrder(CurrentOrder, TableState.OrganizationFilter);
                } else if (CurrentOrder.Contains(TableState.TitleFilter)) {
                    ApplyOrder(CurrentOrder, TableState.TitleFilter);
                } else if (CurrentOrder.Contains(TableState.ModifiedFilter)) {
                    ApplyOrder(CurrentOrder, TableState.ModifiedFilter);
                } else if (CurrentOrder.Contains(TableState.CustomerFilter)) {
                    ApplyOrder(CurrentOrder, TableState.CustomerFilter);
                } else if (CurrentOrder.Contains(TableState.ActivityFilter)) {
                    ApplyOrder(CurrentOrder, TableState.ActivityFilter);
                } else if (CurrentOrder.Contains(TableState.ProducedByFilter)) {
                    ApplyOrder(CurrentOrder, TableState.ProducedByFilter);
                }
            } else {
                // Setting the default order
                CurrentOrder = $"{TableState.ModifiedFilter}_Asc";

                TableState.OrderBy = TableState.ModifiedFilter;
                TableState.OrderMode = ListOrderMode.Desc;
            }

            OrganizationSort = SetOrder(CurrentOrder, TableState.OrganizationFilter);
            TitleSort = SetOrder(CurrentOrder, TableState.TitleFilter);
            ModifiedBySort = SetOrder(CurrentOrder, TableState.ModifiedFilter);
            CustomerSort = SetOrder(CurrentOrder, TableState.CustomerFilter);
            ActivitySort = SetOrder(CurrentOrder, TableState.ActivityFilter);
            ProducedBySort = SetOrder(CurrentOrder, TableState.ProducedByFilter);
        }

        private void ApplyOrder(string sortOrder, string listColumnName) {
            TableState.OrderBy = listColumnName;
            TableState.OrderMode = sortOrder == $"{listColumnName}_Asc" ? ListOrderMode.Desc : ListOrderMode.Asc;
        }

        private string SetOrder(string sortOrder, string listColumnName) {
            return !string.IsNullOrWhiteSpace(sortOrder) && sortOrder.Contains(listColumnName)
                ? sortOrder == $"{listColumnName}_Asc"
                    ? $"{listColumnName}_Desc"
                    : $"{listColumnName}_Asc"
                : $"{listColumnName}";
        }

        #endregion
    }
}
