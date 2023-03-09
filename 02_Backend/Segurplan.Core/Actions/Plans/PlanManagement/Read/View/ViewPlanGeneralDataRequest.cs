using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View {
    public class ViewPlanGeneralDataRequest : ReadPlanGeneralDataRequest, IRequest<IRequestResponse<ReadPlanGeneralDataResponseBase>> {
        public ViewPlanGeneralDataRequest(int id) : base(id) {

        }
    }
}
