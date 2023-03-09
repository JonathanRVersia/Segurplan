using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Articles.ModalList {
    public class ArticlesListRequestHandler : IRequestHandler<ArticlesListRequest, IRequestResponse<ArticlesListResponse>> {
        private readonly SegurplanContext context;
        private readonly IMapper mapper;
        public ArticlesListRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IRequestResponse<ArticlesListResponse>> Handle(ArticlesListRequest request, CancellationToken cancellationToken) {
            var articles = await context.Article
                                        .ProjectTo<ArticlesListResponse.ListItem>(mapper.ConfigurationProvider)
                                        .RunSpecification(request.Specifications);

            return RequestResponse.Ok(new ArticlesListResponse(articles));
        }
    }
}
