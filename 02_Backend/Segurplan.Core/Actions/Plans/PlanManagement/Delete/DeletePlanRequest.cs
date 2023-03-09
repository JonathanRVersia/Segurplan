using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Delete {
    public class DeletePlanRequest : IRequest<IRequestResponse<DeletePlanResponse>> {
        public int PlanId { get; set; }

        public int UserId { get; set; }
    }
}
