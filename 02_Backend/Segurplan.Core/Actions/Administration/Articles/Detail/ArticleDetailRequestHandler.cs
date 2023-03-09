using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Articles.Detail {
    public class ArticleDetailRequestHandler : IRequestHandler<ArticleDetailRequest, IRequestResponse<ArticleDetailResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public ArticleDetailRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IRequestResponse<ArticleDetailResponse>> Handle(ArticleDetailRequest request, CancellationToken cancellationToken) {

            var article = await context.Article
                .ProjectTo<ApplicationArticle>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(art => art.Id == request.Id);

            var family = await context.ArticleFamily
                .ProjectTo<ApplicationArticleFamily>(mapper.ConfigurationProvider).ToListAsync();

            if (article is null) return RequestResponse.NotFound<ArticleDetailResponse>();

            return RequestResponse.Ok(new ArticleDetailResponse(article, family));
        }
    } 
}

