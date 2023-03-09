using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List.Specifications;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.Query;
using Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList.Dtos;


namespace Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList {
    public class RisksAndPreventiveMeasuresListViewComponent : ViewComponent {

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public RisksAndPreventiveMeasuresListViewComponent(IMediator mediator, IMapper mapper) {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public IRequestResponse<ListRisksAndPreventiveMeasuresResponse> RiskAndPreventiveMeasuresResponse { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(RisksAndPreventiveMeasuresListModel risksAndPreventiveMeasuresListModel) {

            await LoadData(risksAndPreventiveMeasuresListModel).ConfigureAwait(true);

            return View(risksAndPreventiveMeasuresListModel);
        }

        private async Task LoadData(RisksAndPreventiveMeasuresListModel risksAndPreventiveMeasuresListModel) {
            RiskAndPreventiveMeasuresResponse = await mediator.Send(new ListRisksAndPreventiveMeasuresRequest() {
                Specifications = GetSpecificationsAsync(risksAndPreventiveMeasuresListModel)
            }).ConfigureAwait(true);

            if (RiskAndPreventiveMeasuresResponse.Status == RequestStatus.Ok) {
                risksAndPreventiveMeasuresListModel.PageSize = RiskAndPreventiveMeasuresResponse.Value.PageSize.Value;
                risksAndPreventiveMeasuresListModel.TotalCount = RiskAndPreventiveMeasuresResponse.Value.TotalCount.Value;

                risksAndPreventiveMeasuresListModel.TableValues.AddRange(mapper.Map<List<RisksAndPreventiveMeasuresTableDataModel>>(RiskAndPreventiveMeasuresResponse.Value.RiskAndPrevMeasures));
            }

        }

        private IEnumerable<ISpecification<ListRisksAndPreventiveMeasuresResponse.ListItem>> GetSpecificationsAsync(RisksAndPreventiveMeasuresListModel risksAndPreventiveMeasuresListModel) {
            var specifications = new List<ISpecification<ListRisksAndPreventiveMeasuresResponse.ListItem>>();

            specifications.AddRange(GetOrder(risksAndPreventiveMeasuresListModel));

            if (risksAndPreventiveMeasuresListModel.IsSearch)
                specifications.AddRange(GetFilters(risksAndPreventiveMeasuresListModel));

            specifications.Add(GetPagination(risksAndPreventiveMeasuresListModel));

            return specifications;
        }


        private IEnumerable<OrderRisksAndPreventiveMeasuresSpecification> GetOrder(RisksAndPreventiveMeasuresListModel risksAndPreventiveMeasuresListModel) {
            List<OrderRisksAndPreventiveMeasuresSpecification> orderBySpecs = new List<OrderRisksAndPreventiveMeasuresSpecification>();

            orderBySpecs.Add(GetOrderFilters(risksAndPreventiveMeasuresListModel));

            return orderBySpecs;
        }


        private OrderRisksAndPreventiveMeasuresSpecification GetOrderFilters(RisksAndPreventiveMeasuresListModel risksAndPreventiveMeasuresListModel) {
            OrderRisksAndPreventiveMeasuresSpecification orderFilters = new OrderRisksAndPreventiveMeasuresSpecification();

            switch (risksAndPreventiveMeasuresListModel.ShortOrder) {

                case "ByChapterNumber_asc":
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "BySubChapterNumber_asc":
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "ByActivityNumber_asc":
                    orderFilters.ByActivityNumber();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    break;
                case "ByActivityDescription_asc":
                    orderFilters.ByActivityDescription();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "ByRisks_asc":
                    orderFilters.ByRisks();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "ByPreventiveMeasureDescription_asc":
                    //orderFilters.ByPreventiveMeasureDescription();
                    break;
                case "ByProbability_asc":
                    orderFilters.ByProbability();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "BySeriousness_asc":
                    orderFilters.BySeriousness();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "ByRiskLevel_asc":
                    orderFilters.ByRiskLevel();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;


                case "ByChapterNumber_desc":
                    orderFilters.ByChapterNumberDesc();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "BySubChapterNumber_desc":
                    orderFilters.BySubChapterNumberDesc();
                    orderFilters.ByChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "ByActivityNumber_desc":
                    orderFilters.ByActivityNumberDesc();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    break;
                case "ByActivityDescription_desc":
                    orderFilters.ByActivityDescriptionDesc();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "ByRisks_desc":
                    orderFilters.ByRisksDesc();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "ByPreventiveMeasureDescription_desc":
                    //orderFilters.ByPreventiveMeasureDescriptionDesc();
                    break;
                case "ByProbability_desc":
                    orderFilters.ByProbabilityDesc();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "BySeriousness_desc":
                    orderFilters.BySeriousnessDesc();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                case "ByRiskLevel_desc":
                    orderFilters.ByRiskLevelDesc();
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
                default:
                    orderFilters.ByChapterNumber();
                    orderFilters.BySubChapterNumber();
                    orderFilters.ByActivityNumber();
                    break;
            }
            return orderFilters;
        }

        private IEnumerable<FilterRisksAndPreventiveMeasuresSpecification> GetFilters(RisksAndPreventiveMeasuresListModel risksAndPreventiveMeasuresListModel) => new List<FilterRisksAndPreventiveMeasuresSpecification> { risksAndPreventiveMeasuresListModel.Search.ToSpecification() };


        public PaginatedRisksAndPreventiveMeasuresSpecification GetPagination(RisksAndPreventiveMeasuresListModel risksAndPreventiveMeasuresListModel) {
            PaginatedRisksAndPreventiveMeasuresSpecification defaultPagination = new PaginatedRisksAndPreventiveMeasuresSpecification(risksAndPreventiveMeasuresListModel.PageNumber, risksAndPreventiveMeasuresListModel.PageSize);

            if (RiskAndPreventiveMeasuresResponse == null)
                return defaultPagination;

            //entra en algun momento?
            int currentPage = RiskAndPreventiveMeasuresResponse.Value.Page.Value;

            PaginatedRisksAndPreventiveMeasuresSpecification paginated = new PaginatedRisksAndPreventiveMeasuresSpecification(currentPage, currentPage + risksAndPreventiveMeasuresListModel.PageSize);

            return paginated;
        }
    }
}
