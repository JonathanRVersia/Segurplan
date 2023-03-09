using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Details {
    public class ChapterDetailsRequest : IRequest<IRequestResponse<ChapterDetailsResponse>> {
        public int ChapterversionId { get; set; }
    }
}
