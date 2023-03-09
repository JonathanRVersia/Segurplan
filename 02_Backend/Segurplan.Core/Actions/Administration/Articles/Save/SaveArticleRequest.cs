using MediatR;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Articles.Save {
    public class SaveArticleRequest : IRequest<IRequestResponse<SaveArticleResponse>> {
        public int UserId { get; set; }
        public ApplicationArticle Article { get; set; }
    }
}
