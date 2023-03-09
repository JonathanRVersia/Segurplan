using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.BusinessManagers;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Articles {
    public class ArticlesListRequestHandler : IRequestHandler<ArticlesListRequest, IRequestResponse<ArticlesListResponse>> {
        protected readonly ArticlesDam articlesDam;
        public ArticlesListRequestHandler(ArticlesDam articlesDam) {
            this.articlesDam = articlesDam;
        }
        public async Task<IRequestResponse<ArticlesListResponse>> Handle(ArticlesListRequest request, CancellationToken cancellationToken) {
            return await GetArticlesList(request);
        }
        private async Task<IRequestResponse<ArticlesListResponse>> GetArticlesList(ArticlesListRequest request) {
            var manager = new ArticlesListManager(articlesDam);
            var articlesList = await manager.GetArticlesList(request.TableState, request.TableFilter);

            return RequestResponse.Ok(new ArticlesListResponse(articlesList, manager.FilteredArticles));
        }
    }
}
