using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View {
    public class ViewPlanActivitiesRequest : IRequest<IRequestResponse<ViewPlanActivitiesResponse>> {

        public int PlanId { get; set; }

        public bool GetAll { get; set; } = true;
    }
}
