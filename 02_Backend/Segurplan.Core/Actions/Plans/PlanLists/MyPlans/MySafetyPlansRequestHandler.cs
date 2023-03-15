using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanLists {
    public class MySafetyPlansRequestHandler : SafetyPlanRequestHandlerBase, IRequestHandler<MySafetyPlanRequest, IRequestResponse<SafetyPlanResponseBase>> {
        public MySafetyPlansRequestHandler(SafetyStudyPlanDam dam) : base(dam) {
        }

        private string UserId { get; set; }

        public Task<IRequestResponse<SafetyPlanResponseBase>> Handle(MySafetyPlanRequest request, CancellationToken cancellationToken) {
            UserId = "3";
            return base.Handle(request, cancellationToken);
        }

        protected override void SetCreationUserConstraint(ref IQueryable<SafetyStudyPlan> consulta) {
            consulta = from plan in consulta
                       where int.Parse(UserId) == plan.CreatedBy
                       select plan;
        }
    }
}
