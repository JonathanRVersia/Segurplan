using System.Collections.Generic;
using MediatR;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.DuplicatePlanCustomChapters {
    public class DuplicatePlanCustomChaptersRequest : IRequest<IRequestResponse<DuplicatePlanCustomChaptersResponse>> {
        public List<PlanActivityVersion> PlanActivityVersions = new List<PlanActivityVersion>();
    }
}
