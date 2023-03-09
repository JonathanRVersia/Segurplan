using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Articles.Delete {
    public class DeleteArticleRequest : IRequest<IRequestResponse<DeleteArticleResponse>> {
        public int Id { get; set; }
    }
}
