using System.Collections.Generic;
using Segurplan.Core.Actions.Plans.PlansData.Activities;

namespace Segurplan.Core.Actions.Plans.PlanManagement.CreatePlanCustomChapters {
    public class CreatePlanCustomChaptersResponse {

        public CreatePlanCustomChaptersResponse(List<SelectedPlanActivity> selectedPlanActivities) {
            SelectedPlanActivities = selectedPlanActivities;
        }

        public List<SelectedPlanActivity> SelectedPlanActivities { get; set; }
    }
}
