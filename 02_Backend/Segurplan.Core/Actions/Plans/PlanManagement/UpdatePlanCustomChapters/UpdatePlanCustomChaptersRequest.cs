using System.Collections.Generic;
using MediatR;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.UpdatePlanCustomChapters {
    public class UpdatePlanCustomChaptersRequest : IRequest<IRequestResponse<UpdatePlanCustomChaptersResponse>> {

        public List<PlanChapter> Chapters { get; set; } = new List<PlanChapter>();
        //public List<PlanSubChapter> CustomSubChapters { get; set; } = new List<PlanSubChapter>();
        //public List<PlanActivity> CustomActivities { get; set; } = new List<PlanActivity>();
    }
}
