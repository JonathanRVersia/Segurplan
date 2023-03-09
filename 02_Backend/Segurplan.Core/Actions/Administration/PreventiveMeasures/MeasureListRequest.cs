using MediatR;
using Segurplan.Core.Actions.Actions.Administration.PreventiveMeasures;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.PreventiveMeasures {
    public class MeasureListRequest : IRequest<IRequestResponse<MeasureListResponse>> {


        public MeasureListTableState TableState { get; set; }
        public PreventiveMeasuresFilter TableFilter { get; set; }

        public MeasureListRequest(MeasureListTableState tableState, PreventiveMeasuresFilter tableFilter) {

            TableState = tableState;
            TableFilter = tableFilter;

        }
    }
}
