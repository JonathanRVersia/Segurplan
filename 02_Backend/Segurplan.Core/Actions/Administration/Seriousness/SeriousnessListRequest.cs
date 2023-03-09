using MediatR;
using Segurplan.Core.Actions.Administration.Seriousness;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Seriousness {
    public class SeriousnessListRequest : IRequest<IRequestResponse<SeriousnessListResponse>> {
        public SeriousnessListTableState TableState { get; set; }
        public SeriousnessFilter TableFilter { get; set; }

        public SeriousnessListRequest(SeriousnessListTableState tableState, SeriousnessFilter tableFilter) {
            TableState = tableState;
            TableFilter = tableFilter;
        }
    }
}
