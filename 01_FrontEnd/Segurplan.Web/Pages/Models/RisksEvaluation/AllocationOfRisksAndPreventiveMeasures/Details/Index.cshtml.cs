using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Segurplan.Core.Actions.Administration.PreventiveMeasures.ModalList;
using Segurplan.Core.Actions.Administration.PreventiveMeasures.ModalList.Specifications;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Delete;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Detail;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.DependantDropdowns;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.Detail;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.RiskLevelByProbabilityAndSeriousness;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Update;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.Query;


namespace Segurplan.Web.Pages.Models.RisksEvaluation.AllocationOfRisksAndPreventiveMeasures.Details {

    [Authorize(Roles = "Administrador, Usuario")]
    public class IndexModel : PageModel {

        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IConfiguration _config;

        public IndexModel(IMediator mediator, IMapper mapper, IConfiguration config) {
            this.mediator = mediator;
            this.mapper = mapper;
            _config = config;
        }

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsEdit { get; set; }

        [BindProperty(SupportsGet = true)]
        public string IndexPage { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchValues { get; set; }

        [BindProperty]
        public string SaveType { get; set; }
        [BindProperty]
        public RisksAndPreventiveMeasuresDetailModel RisksAndPreventiveMeasure { get; set; } = new RisksAndPreventiveMeasuresDetailModel();
        public RiskAndPreventiveMeasuresDetailDropdownResponse Dropdowns { get; set; }
        public IRequestResponse<PreventiveMeasureListResponse> PreventiveMeasureList { get; set; }
        public PreventiveMeasureDetailsDropdownFields DropdownFields { get; set; } = new PreventiveMeasureDetailsDropdownFields();
        public bool RiskIsAlreadyAsigned { get; set; }
        public bool Vigente { get; set; }

        public int PreventiveMeasureModalPageNumber { get; set; } = 1;
        public List<RiskLevelBySeriousnessAndProbabilitiesDto> RiskLevelMatrixData { get; private set; }

        public int PreventiveMeasureModalPageSize = 10;

        private string PreventiveMeasureSearchCode;
        private string PreventiveMeasureSearchDescription;
        private string PreventiveMeasureSearchInitialWord;

        public List<string> SelectedMeasuresCodes { get; set; } = new List<string>();

        public async Task OnGetAsync() {
            await DataLoadAsync().ConfigureAwait(true);
        }

        private async Task DataLoadAsync(List<PreventiveMeasureModel> preventiveMeasures = null) {
            var isEdit = false;
            await LoadRiskAndPreventiveMeasureAsync().ConfigureAwait(true);
            if (preventiveMeasures != null) {
                RisksAndPreventiveMeasure.PreventiveMeasures = preventiveMeasures;
            }
            if (RisksAndPreventiveMeasure.Id != 0) {
                isEdit = true;
            }
            await LoadDetailsDropdownsAsync(isEdit).ConfigureAwait(true);
            await GetDependantDropdownData().ConfigureAwait(true);
            GenerateChaptSubChaptActDropdownFields();
            GenerateRiskOrderDropdownFields();
            await GetMeasuresList().ConfigureAwait(true);
            await GetRiskLevelMatrixData().ConfigureAwait(true);
            await FillSelectedCodes().ConfigureAwait(true);

        }

        private async Task FillSelectedCodes() {
            if (RisksAndPreventiveMeasure.PreventiveMeasures.Any()) {
                SelectedMeasuresCodes = RisksAndPreventiveMeasure.PreventiveMeasures.Select(x => x.PreventiveMeasureCode).ToList();
            }

        }

        private async Task GetRiskLevelMatrixData() {
            var result = await mediator.Send(new RiskLevelByProbabilityAndSeriousnessRequest()).ConfigureAwait(true);
            RiskLevelMatrixData = result.Value.RiskLevelBySeriousnessAndProbabilities;
        }

        public async Task<IActionResult> OnPostMeasuresListPagination(string nextPage, string measureCode, string measureDescription, string initialWord) {

            int page;

            bool success = Int32.TryParse(nextPage, out page);

            if (success) {
                PreventiveMeasureModalPageNumber = page;
                PreventiveMeasureSearchCode = measureCode;
                PreventiveMeasureSearchDescription = measureDescription;
                PreventiveMeasureSearchInitialWord = initialWord;
                await GetMeasuresList().ConfigureAwait(true);
            }

            if (PreventiveMeasureList.Status == RequestStatus.Ok) {
                return new JsonResult(PreventiveMeasureList.Value);
            } else {
                return StatusCode(404);
            }

        }


        private async Task GetMeasuresList() {
            PreventiveMeasureList = await mediator.Send(new PreventiveMeasureListRequest() { Specifications = GetSpecifications() }).ConfigureAwait(true);
        }

        private IEnumerable<ISpecification<PreventiveMeasureListResponse.ListItem>> GetSpecifications() {
            var specifications = new List<ISpecification<PreventiveMeasureListResponse.ListItem>>();

            specifications.Add(GetPagination());

            specifications.Add(GetFilters());
            specifications.Add(GetOrder());
            return specifications;
        }

        private ISpecification<PreventiveMeasureListResponse.ListItem> GetFilters() {
            PreventiveMeasuresSpecification searchFilter = new PreventiveMeasuresSpecification();

            if (!string.IsNullOrEmpty(PreventiveMeasureSearchCode))
                searchFilter.ByCode(PreventiveMeasureSearchCode);
            if (!string.IsNullOrEmpty(PreventiveMeasureSearchDescription))
                searchFilter.ByDescription(PreventiveMeasureSearchDescription);
            if (!string.IsNullOrEmpty(PreventiveMeasureSearchInitialWord))
                searchFilter.ByInitialWord(PreventiveMeasureSearchInitialWord);

            return searchFilter;
        }
        private ISpecification<PreventiveMeasureListResponse.ListItem> GetOrder() {
            OrderByPreventiveMeasureSpecifications orderFilter = new OrderByPreventiveMeasureSpecifications();

            orderFilter.ByCodeDesc();

            return orderFilter;
        }
        private ISpecification<PreventiveMeasureListResponse.ListItem> GetPagination() {
            PaginatedPreventiveMeasuresSpecification defaultPagination = new PaginatedPreventiveMeasuresSpecification(PreventiveMeasureModalPageNumber, PreventiveMeasureModalPageSize);

            if (PreventiveMeasureList == null)
                return defaultPagination;

            //entra en algun momento?
            int currentPage = PreventiveMeasureList.Value.Page.Value;

            PaginatedPreventiveMeasuresSpecification paginated = new PaginatedPreventiveMeasuresSpecification(currentPage, currentPage + PreventiveMeasureModalPageSize);

            return paginated;
        }

        private async Task LoadDetailsDropdownsAsync(bool isEdit) {

            var response = await mediator.Send(new RiskAndPreventiveMeasuresDetailDropdownRequest { IsEdit = isEdit , Vigente = Vigente}).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                Dropdowns = response.Value;
            }

        }

        private async Task GetDependantDropdownData() {
            var result = await mediator.Send(new RiskAndPreventiveMeasuresDependantDropdownsByIdRequest {
                ChapterId = RisksAndPreventiveMeasure.ChapterId,
                SubChapterId = RisksAndPreventiveMeasure.SubChapterId,
                Target = "All"
            }).ConfigureAwait(true);

            if (result.Status != RequestStatus.Ok) {
                Dropdowns.SubChapter = new List<SubChapterDropdownDto>();
                Dropdowns.Activity = new List<ActivityDropdownDto>();

            } else {
                Dropdowns.SubChapter = result.Value.SubChapterVersion;
                Dropdowns.Activity = result.Value.Activities;
            }
        }

        private void GenerateChaptSubChaptActDropdownFields() {
            foreach (var subchapt in Dropdowns.SubChapter) {
                DropdownFields.SubShapterDropdownItems.Add(new SelectListItem {
                    Value = subchapt.IdSubchapter,
                    Text = $"{subchapt.Number} {subchapt.Title}",
                    Selected = RisksAndPreventiveMeasure.SubChapterId == Convert.ToInt32(subchapt.IdSubchapter),
                });
            }

            foreach (var act in Dropdowns.Activity) {
                DropdownFields.ActivityDropdownItems.Add(new SelectListItem {
                    Value = act.Id.ToString(),
                    Text = $"{act.Number} {act.Title}",
                    Selected = RisksAndPreventiveMeasure.ActivityId == act.Id,
                });
            }
        }


        private async Task LoadRiskAndPreventiveMeasureAsync() {
            var response = await mediator.Send(new DetailRiskAndPreventiveMeasuresRequest() { Id = Id.Value }).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                RisksAndPreventiveMeasure = mapper.Map<RisksAndPreventiveMeasuresDetailModel>(response.Value);
                RisksAndPreventiveMeasure.ChapterId = await GetIdChapterVersion(RisksAndPreventiveMeasure.SubChapterId).ConfigureAwait(true);
                if (RisksAndPreventiveMeasure.PreventiveMeasures.Any()) {
                    RisksAndPreventiveMeasure.PreventiveMeasures = RisksAndPreventiveMeasure.PreventiveMeasures.OrderBy(x => x.PreventiveMeasureOrder).ToList();
                }
            }
        }
        private async Task<int> GetIdChapterVersion(int subchapterId) {
            var result = await mediator.Send(new RiskAndPreventiveMeasuresDependantDropdownsByIdRequest {
                ChapterId = RisksAndPreventiveMeasure.ChapterId,
                Target = "chapterVersionByChapter",
                SubChapterId = subchapterId
            }).ConfigureAwait(true);

            if (result.Value != null) {
                Vigente = result.Value.Vigente;
                return result.Value.ChapterVersion.Where(x => x.IdChapter == RisksAndPreventiveMeasure.ChapterId).FirstOrDefault().Id;
            } else {
                Vigente = true;
                return 0;
            }
                
        }

        public async Task<IActionResult> OnPostDelete() {
            if (Id.HasValue) {
                await mediator.Send(new DeleteRiskAndPreventiveMeasureRequest() { RiskAndPreventiveMeasureId = Id.Value }).ConfigureAwait(true);
            }

            return RedirectToPage("../List/Index", "Search", new { SearchValues = SearchValues });
        }
        private async Task<int> GetMainChapterData() {
            var result = await mediator.Send(new RiskAndPreventiveMeasuresDependantDropdownsByIdRequest {
                ChapterVersionId = RisksAndPreventiveMeasure.ChapterId,
                Target = "chapterVersion"
            }).ConfigureAwait(true);

            if (result.Status != RequestStatus.Ok) {
                return 0;

            } else {
                return result.Value.ChapterVersion[0].IdChapter;
            }
        }

        public async Task<IActionResult> OnPostSaveChanges() {
            UpdateRiskAndPreventiveMeasuresModel updateRiskAndPreventiveMeasuresModel = mapper.Map<UpdateRiskAndPreventiveMeasuresModel>(RisksAndPreventiveMeasure);

            updateRiskAndPreventiveMeasuresModel.ChapterId = await GetMainChapterData().ConfigureAwait(true);

            var response = await mediator.Send(new UpdateRiskAndPreventiveMeasuresRequest() {
                RiskAndPreventiveMeasures = updateRiskAndPreventiveMeasuresModel,
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
            }).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                switch (SaveType) {
                    case "saveAndClose":
                        return RedirectToPage("../List/Index", "Search", new { SearchValues = SearchValues });
                    case "saveAndNew":
                        return Redirect(Url.Page("/Models/RisksEvaluation/AllocationOfRisksAndPreventiveMeasures/Details/Index", new { id = 0, isEdit = true, indexPage = IndexPage, SearchValues = SearchValues }));
                    case "save":
                        return Redirect(Url.Page("/Models/RisksEvaluation/AllocationOfRisksAndPreventiveMeasures/Details/Index", new { id = response.Value.RiskPreventiveMeasureId, isEdit = true, indexPage = IndexPage , SearchValues = SearchValues}));
                    //return Redirect($"/Models/RisksEvaluation/AllocationOfRisksAndPreventiveMeasures/Details/Index/{response.Value.RiskPreventiveMeasureId}/true");
                    default:
                        await DataLoadAsync().ConfigureAwait(true);
                        return Page();

                }
            } else {
                if (response.Message == "riskIsAlreadyAsigned") {
                    RiskIsAlreadyAsigned = true;
                }
                await DataLoadAsync(RisksAndPreventiveMeasure.PreventiveMeasures).ConfigureAwait(true);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostClosePage() {
            return RedirectToPage("../List/Index","Search", new { SearchValues = SearchValues });
        }

        private void GenerateRiskOrderDropdownFields() {
            int maxNumber = _config.GetValue<int>("RisksAndPreventiveMeasures:RiskOrderMaxIntOptions");
            for (int i = 1; i < maxNumber; i++) {
                DropdownFields.RiskOrderDropdownItems.Add(new SelectListItem {
                    Value = i.ToString(),
                    Text = i.ToString(),
                });
            }
        }

        public async Task<IActionResult> OnGetDependantData(int selectedId, string target) {

            var result = await mediator.Send(new RiskAndPreventiveMeasuresDependantDropdownsByIdRequest {
                DependantRelationId = selectedId,
                Target = target
            }).ConfigureAwait(true);

            if (result.Status != RequestStatus.Ok) {
                return new StatusCodeResult(400);
            }

            return new JsonResult(result.Value);


        }
    }
}
