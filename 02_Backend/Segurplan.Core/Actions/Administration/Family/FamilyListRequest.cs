using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Family {
    public class FamilyListRequest :IRequest<IRequestResponse<FamilyListResponse>>{

        public FamilyFilter TableFilter { get; set; }
        public FamilyListTableState TableState { get; set; }

        public FamilyListRequest(FamilyListTableState tableState, FamilyFilter tableFilter) {
            this.TableState = tableState;
            this.TableFilter = tableFilter;
        }
    }
}
