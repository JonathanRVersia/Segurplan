using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Details {
    public class ActivityDetailsRequest : IRequest<IRequestResponse<ActivityDetailsResponse>> {
        public int ActivityVersionId { get; set; }
    }
}
