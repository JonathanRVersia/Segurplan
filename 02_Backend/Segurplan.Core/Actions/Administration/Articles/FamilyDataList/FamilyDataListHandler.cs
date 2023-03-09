using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Articles.FamilyDataList {
    class FamilyDataListHandler : IRequestHandler<FamilyDataListRequest, IRequestResponse<FamilyDataListResponse>> {
        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public FamilyDataListHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IRequestResponse<FamilyDataListResponse>> Handle(FamilyDataListRequest request, CancellationToken cancellationToken) {

            var family = await context.ArticleFamily
                .ProjectTo<ApplicationArticleFamily>(mapper.ConfigurationProvider).ToListAsync();

            return RequestResponse.Ok(new FamilyDataListResponse(family));
        }

    }
}
