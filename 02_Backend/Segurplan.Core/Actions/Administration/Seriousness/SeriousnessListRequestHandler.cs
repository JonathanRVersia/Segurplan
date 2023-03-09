using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Actions.Administration.Seriousness;
using Segurplan.Core.BusinessManagers;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Seriousness {
    public class SeriousnessListRequestHandler : IRequestHandler<SeriousnessListRequest, IRequestResponse<SeriousnessListResponse>> {
        protected readonly SeriousnessDam seriousnessDam;

        public SeriousnessListRequestHandler(SeriousnessDam seriousnessDam) {
            this.seriousnessDam = seriousnessDam;
        }

        public async Task<IRequestResponse<SeriousnessListResponse>> Handle(SeriousnessListRequest request, CancellationToken cancellationToken) {
            return await GetSeriousnessList(request);
        }

        private async Task<IRequestResponse<SeriousnessListResponse>> GetSeriousnessList(SeriousnessListRequest request) {
            //try {
                var manager = new SeriousnessListManager(seriousnessDam);
                var seriousnessList = await manager.GetSeriousnessList(request.TableState, request.TableFilter);

                return RequestResponse.Ok(new SeriousnessListResponse(seriousnessList, manager.FilteredSeriousness));

            //} catch (Exception e) {
            //    Debug.WriteLine(e.ToString());
            //    return RequestResponse.NotOk(new SeriousnessListResponse(null, -1));
            //}
        }
    }
}
