using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.GetRelatedChapSubChapActiv {
    public class GetRelatedChapSubChapActResponse {
        public List<ActivityRelationsModel> ActivityRealtionsList { get; set; } = new List<ActivityRelationsModel>();


        public GetRelatedChapSubChapActResponse(List<ActivityRelationsModel> ActivityRealtionsList) {
            this.ActivityRealtionsList = ActivityRealtionsList;
        }
    }
}
