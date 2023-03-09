using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.RelatedActivities.SaveActivityRelations {
    public class SaveActivityRelationsRequest : IRequest<IRequestResponse<SaveActivityRelationsResponse>>{
        public List<ActivityRelationsModel> RelationsDataList { get; set; }
        public int ActivityId { get; set; }
        public int? RelationsId { get; set; }
    }
}
