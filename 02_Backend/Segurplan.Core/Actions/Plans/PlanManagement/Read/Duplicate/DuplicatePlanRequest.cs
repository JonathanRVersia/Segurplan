using MediatR;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.Edit;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.Duplicate {
    public class DuplicatePlanRequest : IRequest<IRequestResponse<EditPlanGeneralDataResponse>> {
        public int OriginalPlanId { get; set; }

        public string PlanTitle { get; set; }

        public int UserId { get; set; }
    }
}
