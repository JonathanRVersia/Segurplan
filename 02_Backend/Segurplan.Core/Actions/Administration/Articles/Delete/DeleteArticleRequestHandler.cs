using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Articles.Delete {
    public class DeleteArticleRequestHandler : IRequestHandler<DeleteArticleRequest, IRequestResponse<DeleteArticleResponse>> {
        protected readonly SegurplanContext context;

        public DeleteArticleRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<DeleteArticleResponse>> Handle(DeleteArticleRequest request, CancellationToken cancellationToken) {
            try {
                context.Article.Remove(new DataAccessLayer.Database.DataTransferObjects.Article { Id = request.Id });
                int deleted = await context.SaveChangesAsync();
                return RequestResponse.Ok(new DeleteArticleResponse());

            } catch (Exception e) {
                return RequestResponse.NotOk(new DeleteArticleResponse());
            }
        }
    }
}
