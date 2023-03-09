using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View {

    public class ViewPlanPlanesRequest : IRequest<IRequestResponse<ViewPlanPlanesResponse>> {
        
        public bool OnlySelected;

        public int PlanId { get; set; }
    }
}
