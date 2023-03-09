using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Seriousness.Detail {
    public class GetSeriousnessDetailRequestHandler : IRequestHandler<GetSeriousnessDetailRequest, IRequestResponse<GetSeriousnessDetailResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public GetSeriousnessDetailRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<GetSeriousnessDetailResponse>> Handle(GetSeriousnessDetailRequest request, CancellationToken cancellationToken) {
            var seriousness = await context.Seriousness
                .Include(s => s.RiskLevelBySeriousnessAndProbabilities)
                .ProjectTo<ApplicationSeriousness>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(s => s.Id == request.Id);

            if (seriousness is null)
                return RequestResponse.NotFound<GetSeriousnessDetailResponse>();

            return RequestResponse.Ok(new GetSeriousnessDetailResponse(seriousness));
        }
    }
}
