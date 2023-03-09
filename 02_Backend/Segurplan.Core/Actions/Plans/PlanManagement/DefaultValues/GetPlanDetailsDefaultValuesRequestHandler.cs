using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.DefaultValues {
    public class GetPlanDetailsDefaultValuesRequestHandler : IRequestHandler<GetPlanDetailsDefaultValuesRequest, IRequestResponse<GetPlanDetailsDefaultValuesResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public GetPlanDetailsDefaultValuesRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<GetPlanDetailsDefaultValuesResponse>> Handle(GetPlanDetailsDefaultValuesRequest request, CancellationToken cancellationToken) {

            var response = await context.PlanDetailsDefaultValues
                                        .ProjectTo<PlanDetailsDefaultValuesDto>(mapper.ConfigurationProvider)
                                        .ToListAsync();

            if (!response.Any())
                return RequestResponse.NotFound<GetPlanDetailsDefaultValuesResponse>();

            return RequestResponse.Ok(new GetPlanDetailsDefaultValuesResponse(response));

        }
    }
}
