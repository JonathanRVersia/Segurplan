using System.Collections.Generic;
using Segurplan.Core.Actions.Plans.PlansData.Activities;

namespace Segurplan.Core.BusinessObjects {
    public class SafetyPlanActivities {

        /// <summary>
        /// Son actividades o capitulos...
        /// </summary>
        public List<PlanChapter> AvailableActivities { get; set; } = new List<PlanChapter>();

        public List<SelectedPlanActivity> PlanActivities { get; set; } = new List<SelectedPlanActivity>();
    }
}
