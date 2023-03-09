using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Delegation {
    public class DelegationRequest : IRequest<IRequestResponse<DelegationResponse>> {

        public int Id { get; private set; }
        public DelegationRequest() : this(-1) {

        }

        public DelegationRequest(int delegationId) {
            Id = delegationId;
        }
    }
}
