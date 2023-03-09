using System.Collections.Generic;
using MediatR;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.GetCustomPlanChapters {
    public class GetCustomPlanChaptersRequest : IRequest<IRequestResponse<GetCustomPlanChaptersResponse>> {

        public List<SelectedPlanActivity> SelectedActivities { get; set; }
    }
}
