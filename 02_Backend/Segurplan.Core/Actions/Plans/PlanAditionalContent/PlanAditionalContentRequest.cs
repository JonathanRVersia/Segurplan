using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanAditionalContent {
    public class PlanAditionalContentRequest : IRequest<IRequestResponse<PlanAditionalContentResponse>> {
        public string PlanID { get; set; }

    }
}
