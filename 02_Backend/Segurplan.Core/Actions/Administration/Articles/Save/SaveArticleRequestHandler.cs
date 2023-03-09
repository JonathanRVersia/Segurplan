using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Articles.Save {
    public class SaveArticleRequestHandler : IRequestHandler<SaveArticleRequest, IRequestResponse<SaveArticleResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public SaveArticleRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IRequestResponse<SaveArticleResponse>> Handle(SaveArticleRequest request, CancellationToken cancellationToken) {
            bool correctSave = false;

            if (request.Article.Id != default) {
                correctSave = await EditBehaviourAsync(request) > 0;
            } else {
                correctSave = await CreateBehaviourAsync(request) > 0;
            }

            if (!correctSave)
                return RequestResponse.Error<SaveArticleResponse>();

            return RequestResponse.Ok(new SaveArticleResponse());
        }
        private async Task<int> CreateBehaviourAsync(SaveArticleRequest request) {
            var article = mapper.Map<Article>(request.Article);
            article.CreatedBy = request.UserId;
            article.ModifiedBy = request.UserId;
            context.Article.Add(article);
            return await context.SaveChangesAsync();
        }
        private async Task<int> EditBehaviourAsync(SaveArticleRequest request) {
            var article = context.Article.FirstOrDefault(r => r.Id == request.Article.Id);

            if (article is null) return default;

            article = mapper.Map<Article>(request.Article);
            MapCustomArticle(article, request);

            context.Article.Update(article);
            return await context.SaveChangesAsync();
        }
        private void MapCustomArticle(Article article, SaveArticleRequest request) {
            article.UpdateDate = DateTime.Now;
            article.ModifiedBy = request.UserId;
        }
    }
}

