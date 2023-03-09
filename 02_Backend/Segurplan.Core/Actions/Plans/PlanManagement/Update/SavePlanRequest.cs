using MediatR;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.Edit;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.MediatR.Validation;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Update {
    public class SavePlanRequest : UpdatePlanRequestBase, IRequest<IRequestResponse<EditPlanGeneralDataResponse>>{
        
    }
}
