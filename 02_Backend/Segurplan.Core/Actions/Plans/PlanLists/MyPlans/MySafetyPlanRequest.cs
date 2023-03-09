using MediatR;
using Segurplan.Core.Actions.Plans.PlansData;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanLists {
    public class MySafetyPlanRequest : SafetyPlanRequestBase, IRequest<IRequestResponse<SafetyPlanResponseBase>> {
        public string UserId { get; }
        public MySafetyPlanRequest(TableState tablestate, Filter filter, string userId) : base(tablestate, filter, userId) {
            UserId = userId;
        }
    }
}
