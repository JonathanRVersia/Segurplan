using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Risks.Detail {
    public class RiskDetailRequestHandler : IRequestHandler<RiskDetailRequest, IRequestResponse<RiskDetailResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public RiskDetailRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IRequestResponse<RiskDetailResponse>> Handle(RiskDetailRequest request, CancellationToken cancellationToken) {

            var risk = await context.Risk
                .ProjectTo<ApplicationRisk>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(r => r.Id == request.Id);

            if (risk is null) return RequestResponse.NotFound<RiskDetailResponse>();

            return RequestResponse.Ok(new RiskDetailResponse(risk));
        }
    }
}
