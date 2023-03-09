using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.GetCustomPlanChapters {
    public class GetCustomPlanChaptersResponse {

        public GetCustomPlanChaptersResponse(List<CustomPlanChapter> customChapters, List<CustomPlanSubchapter> customSubchapters, List<CustomPlanActivity> customActivities) {
            CustomChapters = customChapters;
            CustomSubchapters = customSubchapters;
            CustomActivities = customActivities;
        }
        
        //public GetCustomPlanChaptersResponse(List<CustomPlanChapter> customChapters) {
        //    CustomChapters = customChapters;
        //}

        public List<CustomPlanChapter> CustomChapters { get; set; }

        public List<CustomPlanSubchapter> CustomSubchapters { get; set; }

        public List<CustomPlanActivity> CustomActivities { get; set; }
    }
}
