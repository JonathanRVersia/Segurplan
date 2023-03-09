using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.BusinessManagers;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Risks {
    public class RiskListRequestHandler : IRequestHandler<RiskListRequest, IRequestResponse<RiskListResponse>> {
        
        protected readonly RiskDam riskDam;

        public RiskListRequestHandler(RiskDam riskDam) {
            this.riskDam = riskDam;
        }

        public async Task<IRequestResponse<RiskListResponse>> Handle(RiskListRequest request, CancellationToken cancellationToken) {
            return await GetRiskList(request);
        }

        private async Task<IRequestResponse<RiskListResponse>> GetRiskList(RiskListRequest request) {
            
            var manager = new RiskListManager(riskDam);
            var RiskList = await manager.GetRiskList(request.TableState, request.TableFilter);

            return RequestResponse.Ok(new RiskListResponse(RiskList, manager.FilteredRisk));

        }
    }
}
