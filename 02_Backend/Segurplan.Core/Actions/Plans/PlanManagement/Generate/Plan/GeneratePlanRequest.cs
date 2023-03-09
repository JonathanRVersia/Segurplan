using System.Collections.Generic;
using MediatR;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Generate.Plan {

    public class GeneratePlanRequest : IRequest<IRequestResponse<GeneratePlanResponse>> {

        public int PlanId { get; set; }

        public string TemplateName { get; set; }

        public bool IsEvaluation { get; set; }

        public List<PlanChapter> SelectedActivities { get; set; }
    }
}
