using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Details {
    public class ActivityDetailsResponse {
        public ActivityDetailsActivityVersion ActivityVersion { get; set; }

        public ActivityDetailsResponse(ActivityDetailsActivityVersion activityVersion) {
            ActivityVersion = activityVersion;
        }
    }
}
