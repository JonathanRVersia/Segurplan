using System.Collections.Generic;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.DeleteCheck {
    public class DeleteCheckActivityRequest : IRequest<IRequestResponse<DeleteCheckActivityResponse>> {
        public int ActivityId { get; set; }

        public List<int> ActivityIds { get; set; }

        public List<int> SubChapterIds { get; set; }
    }
}
