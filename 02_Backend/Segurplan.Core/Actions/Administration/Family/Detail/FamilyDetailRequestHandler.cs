using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Family.Detail {
    public class FamilyDetailRequestHandler : IRequestHandler<FamilyDetailRequest, IRequestResponse<FamilyDetailResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public FamilyDetailRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<FamilyDetailResponse>> Handle(FamilyDetailRequest request, CancellationToken cancellationToken) {
            var family = await context.ArticleFamily
                .ProjectTo<ApplicationFamily>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (family is null) return RequestResponse.NotFound<FamilyDetailResponse>();

            return RequestResponse.Ok(new FamilyDetailResponse(family));
        }
    }
}
