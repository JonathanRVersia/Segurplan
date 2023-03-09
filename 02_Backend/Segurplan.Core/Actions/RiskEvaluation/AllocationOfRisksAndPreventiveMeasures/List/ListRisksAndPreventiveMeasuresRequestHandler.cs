using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List {
    public class ListRisksAndPreventiveMeasuresRequestHandler : IRequestHandler<ListRisksAndPreventiveMeasuresRequest, IRequestResponse<ListRisksAndPreventiveMeasuresResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public ListRisksAndPreventiveMeasuresRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<ListRisksAndPreventiveMeasuresResponse>> Handle(ListRisksAndPreventiveMeasuresRequest request, CancellationToken cancellationToken) {

            var riskAndPrevMeasures = context.RisksAndPreventiveMeasures
                                      .ProjectTo<ListRisksAndPreventiveMeasuresResponse.ListItem>(mapper.ConfigurationProvider)
                                      .RunSpecificationSync(request.Specifications);

            if (!riskAndPrevMeasures.Results.Any())
                return RequestResponse.NotFound<ListRisksAndPreventiveMeasuresResponse>();

            return RequestResponse.Ok(new ListRisksAndPreventiveMeasuresResponse(riskAndPrevMeasures));
        }


    }
}
