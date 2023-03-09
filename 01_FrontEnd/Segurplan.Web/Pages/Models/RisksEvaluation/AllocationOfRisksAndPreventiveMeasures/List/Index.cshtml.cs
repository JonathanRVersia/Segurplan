using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Delete;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.DependantDropdowns;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.List;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList;
using Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList.Dtos;
using Segurplan.Web.Utils;

namespace Segurplan.Web.Pages.Models.RisksEvaluation.AllocationOfRisksAndPreventiveMeasures.List {

    [Authorize(Roles = "Administrador, Usuario")]
    public class IndexModel : PageModel {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public IndexModel(IMediator mediator, IMapper mapper) {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public RisksAndPreventiveMeasuresListModel RisksAndPreventiveMeasuresListModel { get; set; } = new RisksAndPreventiveMeasuresListModel();

        public RiskAndPreventiveMeasuresListDropdownResponse SearchDropdowns { get; private set; }
        [BindProperty]
        public RiskAndPreventiveMeasuresSearch Search { get; set; }
        [BindProperty]
        public int PageNumber { get; set; } = 1;
        [BindProperty]
        public int PageSize { get; set; } = 15;
        [BindProperty]
        public int LastPageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchValues { get; set; }

        public const string IndexRoute = "/Models/RisksEvaluation/AllocationOfRisksAndPreventiveMeasures/List/Index";
        public const string DetailsRoute = "/Models/RisksEvaluation/AllocationOfRisksAndPreventiveMeasures/Details/Index";
        public const string IndexPage = "/RisksAndPreventiveMeasures";

        public async Task OnGetAsync(string order = "", string search = "") {
            RisksAndPreventiveMeasuresListModel.IsSearch = false;
            RisksAndPreventiveMeasuresListModel.ShortOrder = order;
            if (string.IsNullOrEmpty(search)) {
                await FillCommonBaseData().ConfigureAwait(true);
                FillPaginationData();
            } else {
                await OnGetSearchAsync(search).ConfigureAwait(true);
            }
        }

        public async Task OnGetDeleteAsync(int id) {
            await mediator.Send(new DeleteRiskAndPreventiveMeasureRequest() { RiskAndPreventiveMeasureId = id }).ConfigureAwait(true);
            await OnGetSearchAsync(SearchValues).ConfigureAwait(true);
        }

        public async Task OnPostSearchAsync() {
            await FillCommonBaseData().ConfigureAwait(true);
            FillPaginationData();
            FillSearchData();
            PassSearchData();
        }
        public async Task OnGetSearchAsync(string SearchValues = "") {
            if (!string.IsNullOrEmpty(SearchValues)) {
                RisksAndPreventiveMeasuresListModel.SearchValues = SearchValues;
                int count = SearchValues.Length - SearchValues.Replace("/", "").Length;
                Search = JsonConvert.DeserializeObject< RiskAndPreventiveMeasuresSearch>(JsonConvert.SerializeObject(RisksAndPreventiveMeasuresListModel.Search));
                if (count > 0) {
                    var searchIds = SearchValues.Split("/");
                    var chapterDigit = Regex.Match(searchIds[0], @"\d+").Value;
                    if (chapterDigit.Length== searchIds[0].Length) {
                        if (!string.IsNullOrEmpty(searchIds[0])) {
                            Search.ChapterId = int.Parse(searchIds[0]);
                            Search.IsBorrador = false;
                        }
                    } else {
                        if (!string.IsNullOrEmpty(searchIds[0])){
                            Search.ChapterId = int.Parse(chapterDigit);
                            Search.IsBorrador = true;
                        }
                    }
                    if (searchIds.Length > 1) {
                        if (!string.IsNullOrEmpty(searchIds[1]))
                            Search.SubChapterId = int.Parse(searchIds[1]);
                    }
                    if (searchIds.Length > 2) {
                        if (!string.IsNullOrEmpty(searchIds[2]))
                            Search.ActivityId = int.Parse(searchIds[2]);                        
                    }
                    if (searchIds.Length > 3) {
                        if (!string.IsNullOrEmpty(searchIds[3]))
                            Search.RiskId = int.Parse(searchIds[3]);
                    }
                    if (searchIds.Length > 4)
                        Search.MeasureCode = int.Parse(searchIds[4]);
                }
                else if (SearchValues.Length>0) {
                    var chapterDigit = Regex.Match(SearchValues, @"\d+").Value;
                    if (chapterDigit.Length == SearchValues.Length) {
                        int.TryParse(SearchValues, out int result);
                        Search.ChapterId = result;
                        Search.IsBorrador = false;
                    } else {
                        int.TryParse(chapterDigit, out int result);
                        Search.ChapterId = result;
                        Search.IsBorrador = true;
                    }
                }
                await FillCommonBaseData().ConfigureAwait(true);
                FillPaginationData();
                FillSearchData();
                if (RisksAndPreventiveMeasuresListModel.Search.SubChapterId == null && Search.SubChapterId != 0) {
                    RisksAndPreventiveMeasuresListModel.Search.SubChapterId = Search.SubChapterId;
                }
                if (RisksAndPreventiveMeasuresListModel.Search.ActivityId == null && Search.ActivityId != 0) {
                    RisksAndPreventiveMeasuresListModel.Search.ActivityId = Search.ActivityId;                    
                }
                FillSearchNumberData();
            } else {
                Search = new RiskAndPreventiveMeasuresSearch();
                await FillCommonBaseData().ConfigureAwait(true);
                FillPaginationData();
                FillSearchData();
            }
        }
        public async Task OnPostChangePageAsync() {
            await FillCommonBaseData().ConfigureAwait(true);
            RisksAndPreventiveMeasuresListModel.PageSize = PageSize;

            if (PageSize != LastPageSize) {
                RisksAndPreventiveMeasuresListModel.PageNumber = 0;
            } else {
                RisksAndPreventiveMeasuresListModel.PageNumber = PageNumber;
            }

            if (Search != null) {
                FillSearchData();
            }
        }

        private async Task FillCommonBaseData() {
            RisksAndPreventiveMeasuresListModel.IndexRoute = IndexRoute;
            RisksAndPreventiveMeasuresListModel.DetailsRoute = DetailsRoute;
            RisksAndPreventiveMeasuresListModel.IndexPage = IndexPage;
            await GetSearchDropdownDataAsync().ConfigureAwait(true);
        }
        private void FillPaginationData() {
            RisksAndPreventiveMeasuresListModel.PageNumber = PageNumber;
            RisksAndPreventiveMeasuresListModel.PageSize = PageSize;
        }
        private void FillSearchData() {
            var borradorExist = RisksAndPreventiveMeasuresListModel.Search.BorradorExist;
            var subchapterId = RisksAndPreventiveMeasuresListModel.Search.SubChapterId;
            var activityId = RisksAndPreventiveMeasuresListModel.Search.ActivityId;
            var subchapteridList = RisksAndPreventiveMeasuresListModel.Search.SubchapterIdList;

            RisksAndPreventiveMeasuresListModel.IsSearch = true;
            RisksAndPreventiveMeasuresListModel.Search = Search;

            RisksAndPreventiveMeasuresListModel.Search.BorradorExist = borradorExist;

            if (subchapterId != 0) 
                RisksAndPreventiveMeasuresListModel.Search.SubChapterId = subchapterId;

            if (activityId != 0) {
                RisksAndPreventiveMeasuresListModel.Search.ActivityId = activityId;
            }

            if (subchapteridList.Count > 0)
                RisksAndPreventiveMeasuresListModel.Search.SubchapterIdList = subchapteridList;

            if (RisksAndPreventiveMeasuresListModel.Search.ChapterId == null)
                RisksAndPreventiveMeasuresListModel.Search.IsBorrador = false;
        }

        private async Task GetSearchDropdownDataAsync() {
            var response = await mediator.Send(new RiskAndPreventiveMeasuresListDropdownRequest {
                SubChapterId = Search?.SubChapterId ?? default(int),
                ChapterId = Search?.ChapterId ?? default(int),
                ActivityId = Search?.ActivityId ?? default(int),
                Borrador = Search?.IsBorrador ?? false
            }).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                RisksAndPreventiveMeasuresListModel.Search.BorradorExist= response.Value.BorradorExist;
                RisksAndPreventiveMeasuresListModel.Search.SubChapterId= response.Value.SubChapterId;
                RisksAndPreventiveMeasuresListModel.Search.ActivityId = response.Value.ActivityId;
                RisksAndPreventiveMeasuresListModel.Search.SubchapterIdList = response.Value.SubchapterIdList;
                SearchDropdowns = response.Value;                
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
        private void PassSearchData() {
            SearchValues = "";
            if (RisksAndPreventiveMeasuresListModel.Search.ChapterId != null) {
                SearchValues += $"{RisksAndPreventiveMeasuresListModel.Search.ChapterId}";
                if(RisksAndPreventiveMeasuresListModel.Search.IsBorrador)
                    SearchValues += $"b";
            }

            if (RisksAndPreventiveMeasuresListModel.Search.SubChapterId != null) {
                SearchValues += $"/{RisksAndPreventiveMeasuresListModel.Search.SubChapterId}";
            } else {
                SearchValues += "/";
            }

            if (RisksAndPreventiveMeasuresListModel.Search.ActivityId != null) {
                SearchValues += $"/{RisksAndPreventiveMeasuresListModel.Search.ActivityId}";
            } else {
                SearchValues += "/";
            }
            if (RisksAndPreventiveMeasuresListModel.Search.RiskId != null) {
                SearchValues += $"/{RisksAndPreventiveMeasuresListModel.Search.RiskId}";
            }
            else{
                SearchValues += "/";
            }
            if (RisksAndPreventiveMeasuresListModel.Search.MeasureDescription != null) {
                SearchValues += $"/{RisksAndPreventiveMeasuresListModel.Search.MeasureCode}";
            }
            if (!string.IsNullOrEmpty(SearchValues) && SearchValues!="///") {
                RisksAndPreventiveMeasuresListModel.SearchValues = SearchValues;
            }
        }
        private void FillSearchNumberData() {

            if (RisksAndPreventiveMeasuresListModel.Search.ChapterId != null) {
                RisksAndPreventiveMeasuresListModel.Search.ChapterNumber = SearchDropdowns.Chapter.Where(x => x.Id == RisksAndPreventiveMeasuresListModel.Search.ChapterId).FirstOrDefault()?.Number;
            }
            if (RisksAndPreventiveMeasuresListModel.Search.SubChapterId!=null)
            {
                RisksAndPreventiveMeasuresListModel.Search.SubChapterNumber = SearchDropdowns.SubChapterCurrent.Where(x => x.IdSubchapter == RisksAndPreventiveMeasuresListModel.Search.SubChapterId.ToString()).FirstOrDefault()?.Number;
            }
            if (RisksAndPreventiveMeasuresListModel.Search.ActivityId != null) {
                RisksAndPreventiveMeasuresListModel.Search.ActivityNumber = SearchDropdowns.ActivityCurrent.Where(x => x.Id == RisksAndPreventiveMeasuresListModel.Search.ActivityId).FirstOrDefault()?.Number;
            }
            if (RisksAndPreventiveMeasuresListModel.Search.RiskId != null) {
                RisksAndPreventiveMeasuresListModel.Search.RiskCode = SearchDropdowns.Risk.Where(x => x.Id == RisksAndPreventiveMeasuresListModel.Search.RiskId).FirstOrDefault()?.Code;
            }
            if (RisksAndPreventiveMeasuresListModel.Search.MeasureCode != null) {
                RisksAndPreventiveMeasuresListModel.Search.MeasureDescription = SearchDropdowns.Measure.Where(x => x.Code == RisksAndPreventiveMeasuresListModel.Search.MeasureCode.ToString()).FirstOrDefault()?.Description;
            }
        }
    }
}
