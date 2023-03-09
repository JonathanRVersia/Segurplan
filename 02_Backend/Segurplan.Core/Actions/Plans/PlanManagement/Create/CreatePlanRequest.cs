using System.Collections.Generic;
using MediatR;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.Edit;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Create {
    public class CreatePlanRequest : IRequest<IRequestResponse<EditPlanGeneralDataResponse>> {
        public SafetyPlan PlanInformation { get; set; }

        public int UserId { get; set; }

    }
}
