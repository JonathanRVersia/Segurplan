using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Articles.Detail {
    public class ArticleDetailRequest : IRequest<IRequestResponse<ArticleDetailResponse>> {
        public int Id { get; set; }
    }
}
