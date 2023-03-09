using MediatR;
using Segurplan.Core.Actions.Plans.PlansData;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanLists {
    public class AllSafetyPlanRequest : SafetyPlanRequestBase, IRequest<IRequestResponse<SafetyPlanResponseBase>> {
        public AllSafetyPlanRequest(TableState tablestate, Filter filter, string userID) : base(tablestate, filter, userID) {
        }
    }
}
