using MediatR;
using Segurplan.Core.Actions.Administration.Templates;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Templates {
    public class TemplateListRequest : IRequest<IRequestResponse<TemplateListResponse>> {
        public TemplateListTableState TableState { get; set; }
        public TemplatesFilter TableFilter { get; set; }

        public TemplateListRequest(TemplateListTableState tableState, TemplatesFilter tableFilter) {
            TableState = tableState;
            TableFilter = tableFilter;
        }
    }
}
