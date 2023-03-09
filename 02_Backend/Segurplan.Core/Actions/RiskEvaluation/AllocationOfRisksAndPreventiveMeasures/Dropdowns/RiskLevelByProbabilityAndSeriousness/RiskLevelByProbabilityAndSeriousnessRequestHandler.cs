using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.RiskLevelByProbabilityAndSeriousness {
    public class RiskLevelByProbabilityAndSeriousnessRequestHandler : IRequestHandler<RiskLevelByProbabilityAndSeriousnessRequest, IRequestResponse<RiskLevelByProbabilityAndSeriousnessResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public RiskLevelByProbabilityAndSeriousnessRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<RiskLevelByProbabilityAndSeriousnessResponse>> Handle(RiskLevelByProbabilityAndSeriousnessRequest request, CancellationToken cancellationToken) {
            var riskLevelBySeriousnessAndProbabilities = await context.RiskLevelBySeriousnessAndProbabilities
                                        .ProjectTo<RiskLevelBySeriousnessAndProbabilitiesDto>(mapper.ConfigurationProvider)
                                        .ToListAsync();

            return RequestResponse.Ok(new RiskLevelByProbabilityAndSeriousnessResponse(riskLevelBySeriousnessAndProbabilities));
        }
    }
}
