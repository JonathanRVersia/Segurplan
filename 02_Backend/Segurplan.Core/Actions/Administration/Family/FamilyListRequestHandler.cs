using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.BusinessManagers;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Family {
    public class FamilyListRequestHandler : IRequestHandler<FamilyListRequest, IRequestResponse<FamilyListResponse>> {

        protected readonly FamilyDam familyDam;

        public FamilyListRequestHandler(FamilyDam familyDam) {
            this.familyDam = familyDam;
        }

        public async Task<IRequestResponse<FamilyListResponse>> Handle(FamilyListRequest request, CancellationToken cancellationToken) {
            return await GetFamilyList(request);
        }

        private async Task<IRequestResponse<FamilyListResponse>> GetFamilyList(FamilyListRequest request) {
            var manager = new FamilyListManager(familyDam);
            var FamilyList = await manager.GetFamilyList(request.TableState, request.TableFilter);

            return RequestResponse.Ok(new FamilyListResponse(FamilyList, manager.FilteredFamily));
        }
    }
}
