using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Articles {
    public class ArticlesListRequest : IRequest<IRequestResponse<ArticlesListResponse>> { 
        public ArticlesListTableState TableState { get; set; }
        public ArticlesFilter TableFilter { get; set; }

        public ArticlesListRequest(ArticlesListTableState tableState, ArticlesFilter tableFilter) {
            TableState = tableState;
            TableFilter = tableFilter;
        }
    }
}
