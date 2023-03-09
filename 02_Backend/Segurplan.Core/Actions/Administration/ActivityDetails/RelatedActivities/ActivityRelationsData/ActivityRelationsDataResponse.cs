using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.RelatedActivities.ActivityRelationsData {
    public class ActivityRelationsDataResponse {
        public List<ActivityRelationsModel> ActivityRelationsList { get; set; } = new List<ActivityRelationsModel>();


        public ActivityRelationsDataResponse(List<ActivityRelationsModel> ActivityRelationsList) {
            this.ActivityRelationsList = ActivityRelationsList;
        }
    }
}
