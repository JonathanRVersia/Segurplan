using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Actions.Plans.PlansData;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanAditionalContent {
    public class PlanAditionalContentRequestHandler : IRequestHandler<PlanAditionalContentRequest, IRequestResponse<PlanAditionalContentResponse>> {
        private readonly SafetyStudyPlanDam dam;
        public PlanAditionalContentRequestHandler(SafetyStudyPlanDam dam) {
            this.dam = dam;
        }
        public async Task<IRequestResponse<PlanAditionalContentResponse>> Handle(PlanAditionalContentRequest request, CancellationToken cancellationToken) {

            return RequestResponse.Ok(new PlanAditionalContentResponse(new TabContenidos().ToString()));


        }
    }
}
