using System.Collections.Generic;
using Segurplan.Core.Actions.Plans.PlansData.Activities;

namespace Segurplan.Web.Pages.Components.SelectedActivitiesPlanList {
    public class SelectedActivitiesPlanListsModel {
        public List<PlanChapter> SelectedChapters { get; set; }
        public bool IsEditEnabled { get; set; }
    }
}
