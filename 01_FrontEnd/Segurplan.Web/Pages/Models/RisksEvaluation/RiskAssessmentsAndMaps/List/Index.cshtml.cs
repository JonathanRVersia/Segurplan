using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.View;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.Core.Actions.RiskEvaluation.EvaluationsOfRisksAndPreventiveMeasures.Generate.RiskMap;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.Web.Pages.Components.Activities;
using Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList;
using Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList.Dtos;

namespace Segurplan.Web.Pages.Models.RisksEvaluation.RiskAssessmentsAndMaps.List {

    [Authorize(Roles = "Administrador, Usuario")]
    public class IndexModel : PageModel {

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public IndexModel(IMediator mediator, IMapper mapper) {
            this.mediator = mediator;
            this.mapper = mapper;
        }


        public RisksAndPreventiveMeasuresListModel RisksAndPreventiveMeasuresListModel { get; set; } = new RisksAndPreventiveMeasuresListModel();
        public ActivitiesModel activitiesModel { get; set; } = new ActivitiesModel();


        [BindProperty]
        public int PageNumber { get; set; } = 1;
        [BindProperty]
        public int PageSize { get; set; } = 15;
        [BindProperty]
        public int LastPageSize { get; set; }
        [BindProperty]
        public List<ChaptSubchaptActIdsFilterModel> FilterData { get; set; } = new List<ChaptSubchaptActIdsFilterModel>();


        public bool CanGenerateDocs { get; set; }


        [BindProperty]
        public string TargetTemplate { get; set; }
        [BindProperty]
        public string Title { get; set; } = "";

        public const string IndexRoute = "/Models/RisksEvaluation/RiskAssessmentsAndMaps/List/Index";
        public const string DetailsRoute = "/Models/RisksEvaluation/AllocationOfRisksAndPreventiveMeasures/Details/Index";
        public const string IndexPage = "/RiskAssessmentsAndMaps";



        public async Task OnGetAsync(string order = "") {
            RisksAndPreventiveMeasuresListModel.IsSearch = false;
            RisksAndPreventiveMeasuresListModel.ShortOrder = order;
            FillCommonBaseData();
            FillPaginationData();
        }

        public async Task OnPostChangePageAsync() {
            FillCommonBaseData();
            RisksAndPreventiveMeasuresListModel.PageSize = PageSize;

            if (PageSize != LastPageSize) {
                RisksAndPreventiveMeasuresListModel.PageNumber = 1;
            } else {
                RisksAndPreventiveMeasuresListModel.PageNumber = PageNumber;
            }

            if (FilterData != null) {
                FillSearchData();
            }
        }

        public async Task OnPostRunFiltersAsync() {
            FillCommonBaseData();
            FillPaginationData();
            FillSearchData();
        }

        private void FillSearchData() {
            if (FilterData.Any()) {
                RisksAndPreventiveMeasuresListModel.IsSearch = true;
                RisksAndPreventiveMeasuresListModel.Search.ChapSubChaptFilter = mapper.Map<List<ChaptSubChaptActFilterData>>(FilterData);
            }
        }

        private void FillPaginationData() {
            RisksAndPreventiveMeasuresListModel.PageNumber = PageNumber;
            RisksAndPreventiveMeasuresListModel.PageSize = PageSize;
        }

        private void FillCommonBaseData() {
            CanGenerateDocs = FilterData.Any();
            RisksAndPreventiveMeasuresListModel.IndexRoute = IndexRoute;
            RisksAndPreventiveMeasuresListModel.DetailsRoute = DetailsRoute;
            RisksAndPreventiveMeasuresListModel.IndexPage = IndexPage;

            activitiesModel.IndexPage = IndexPage;
            activitiesModel.MoveHidden = true;
        }

        public async Task<JsonResult> OnGetActivitiesAsync() {

            var response = await mediator.Send(new ViewPlanActivitiesRequest {

                PlanId = 0,
                GetAll = true

            }).ConfigureAwait(true);


            if (response.Status == RequestStatus.Ok) {
                return new JsonResult(response.Value.ActivityLists);

            } else {
                return new JsonResult("error") { StatusCode = 404 };

            }
        }

        public async Task<IActionResult> OnPostCreateDocument(List<ChaptSubChaptActFilterData> SelectedData,string TargetTemplate, string Title) {
            if (SelectedData.Any()) {

                var response = await mediator.Send(new GenerateEvaluationOfRisksDocsRequest() {
                    TargetTemplate = TargetTemplate,
                    FilterData = /*mapper.Map<List<ChaptSubChaptActFilterData>>(*/SelectedData/*)*/,
                    Title = Title
                }).ConfigureAwait(false);

                if (response.Status == RequestStatus.Ok) {
                    return new FileStreamResult(
                        response.Value.ResponseStream,
                        new MediaTypeHeaderValue(response.Value.MediaType)
                        ) {
                        FileDownloadName = response.Value.OutputFileName
                    };
                } else {
                    return new NoContentResult();
                }
            } else{
                return new NoContentResult();
            }
        }

    }
}
