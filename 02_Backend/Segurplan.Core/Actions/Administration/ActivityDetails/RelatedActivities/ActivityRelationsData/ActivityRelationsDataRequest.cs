using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.RelatedActivities.ActivityRelationsData {
    public class ActivityRelationsDataRequest : IRequest<IRequestResponse<ActivityRelationsDataResponse>>{
        public List<ActivityRelationsModel> RelationsDataList { get; set; }
    }
}
