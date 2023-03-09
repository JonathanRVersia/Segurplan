using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.DeleteCheck {
    public class DeleteCheckChapterRequest : IRequest<IRequestResponse<DeleteCheckChapterResponse>> {
        public int ChapterId { get; set; }
    }
}
