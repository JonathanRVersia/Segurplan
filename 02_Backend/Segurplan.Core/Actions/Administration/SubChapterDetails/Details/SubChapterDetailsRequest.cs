using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Details {
    public class SubChapterDetailsRequest : IRequest<IRequestResponse<SubChapterDetailsResponse>> {
        public int SubChapterversionId { get; set; }
    }
}
