using System.Collections.Generic;
using MediatR;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.CreatePlanCustomChapters {
    public class CreatePlanCustomChaptersRequest : IRequest<IRequestResponse<CreatePlanCustomChaptersResponse>> {

        public List<PlanChapter> Chapters { get; set; } = new List<PlanChapter>();
        //public List<PlanSubChapter> NewCustomSubChapters { get; set; } = new List<PlanSubChapter>();
        //public List<PlanActivity> NewCustomActivities { get; set; } = new List<PlanActivity>();
    }
}
