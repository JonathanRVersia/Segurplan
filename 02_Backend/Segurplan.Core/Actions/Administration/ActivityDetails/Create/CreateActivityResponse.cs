using Segurplan.Core.Actions.Administration.ActivityDetails.Models;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Create {
    public class CreateActivityResponse {
        public ActivityDetailsActivityVersion ActivityVersion { get; set; }

        public CreateActivityResponse(ActivityDetailsActivityVersion activityVersion) {
            ActivityVersion = activityVersion;
        }
    }
}
