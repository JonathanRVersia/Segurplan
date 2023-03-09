using MediatR;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Update {
    public class UpdatePlanRequest : UpdatePlanRequestBase, IRequest<IRequestResponse<UpdatePlanResponse>> {
        
    }
}
