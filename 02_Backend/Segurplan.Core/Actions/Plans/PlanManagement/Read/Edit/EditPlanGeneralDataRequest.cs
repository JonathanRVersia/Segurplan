using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.Edit {
    public class EditPlanGeneralDataRequest : ReadPlanGeneralDataRequest, IRequest<IRequestResponse<EditPlanGeneralDataResponse>> {

        public EditPlanGeneralDataRequest(int planId) : base(planId) {

        }
    }
}
