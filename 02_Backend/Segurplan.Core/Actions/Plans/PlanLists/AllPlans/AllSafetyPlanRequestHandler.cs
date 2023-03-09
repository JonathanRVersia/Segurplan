using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanLists {
    public class AllSafetyPlanRequestHandler : SafetyPlanRequestHandlerBase, IRequestHandler<AllSafetyPlanRequest, IRequestResponse<SafetyPlanResponseBase>> {
        public AllSafetyPlanRequestHandler(SafetyStudyPlanDam dam) : base(dam) {
        }

        public Task<IRequestResponse<SafetyPlanResponseBase>> Handle(AllSafetyPlanRequest request, CancellationToken cancellationToken) {
            return base.Handle(request, cancellationToken);
        }

        protected override void SetCreationUserConstraint(ref IQueryable<SafetyStudyPlan> consulta) {
            consulta = (from plan in consulta
                        select plan);
        }

    }
}
