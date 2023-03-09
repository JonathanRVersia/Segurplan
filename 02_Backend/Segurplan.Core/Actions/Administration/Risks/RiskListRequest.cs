using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Risks {
    public class RiskListRequest : IRequest<IRequestResponse<RiskListResponse>>{

        public RiskFilter TableFilter { get; set; }
        public RiskListTableState TableState { get; set; }

        public RiskListRequest(RiskListTableState tableState,RiskFilter tableFilter) {
            this.TableState = tableState;
            this.TableFilter = tableFilter;
        }
    }
}
