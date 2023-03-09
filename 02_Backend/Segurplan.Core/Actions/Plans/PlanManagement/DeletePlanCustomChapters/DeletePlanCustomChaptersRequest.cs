using MediatR;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.DeletePlanCustomChapters {
    public class DeletePlanCustomChaptersRequest : IRequest<IRequestResponse> {

        public SafetyPlan Plan { get; set; }
    }
}
