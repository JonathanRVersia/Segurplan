using System.Threading;
using System.Threading.Tasks;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Delegation {
    public class DelegationRequestHandler {
        private readonly DelegationDam myDam;

        public DelegationRequestHandler(DelegationDam dam) {
            myDam = dam;
        }

        public async Task<IRequestResponse<DelegationResponse>> Handle(DelegationRequest request, CancellationToken cancelToken) {
            if (request.Id > 0) {

            } else {
                myDam.SelectAll();
            }
            return null;
        }
    }
}
