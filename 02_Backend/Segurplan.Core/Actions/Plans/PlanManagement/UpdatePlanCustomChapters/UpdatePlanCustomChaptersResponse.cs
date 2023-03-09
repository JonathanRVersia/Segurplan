using System.Collections.Generic;
using Segurplan.Core.Actions.Plans.PlansData.Activities;

namespace Segurplan.Core.Actions.Plans.PlanManagement.UpdatePlanCustomChapters {
    public class UpdatePlanCustomChaptersResponse {

        public UpdatePlanCustomChaptersResponse(List<SelectedPlanActivity> selectedPlanActivities) {
            SelectedPlanActivities = selectedPlanActivities;
        }

        public List<SelectedPlanActivity> SelectedPlanActivities { get; set; }
    }
}
