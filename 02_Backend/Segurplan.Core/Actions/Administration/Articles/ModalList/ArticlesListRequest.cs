using System.Collections.Generic;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.Articles.ModalList {
    public class ArticlesListRequest : IRequest<IRequestResponse<ArticlesListResponse>> {
        public IEnumerable<ISpecification<ArticlesListResponse.ListItem>> Specifications { get; set; }
    }
}
