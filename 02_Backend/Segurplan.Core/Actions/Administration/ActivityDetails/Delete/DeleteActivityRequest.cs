using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Delete {
    public class DeleteActivityRequest : IRequest<IRequestResponse<DeleteActivityResponse>> {
        public int ActivityId { get; set; }
    }
}
