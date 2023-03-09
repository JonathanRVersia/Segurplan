using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Delete {
    public class DeleteChapterRequest : IRequest<IRequestResponse<DeleteChapterResponse>> {
        public int ChapterId { get; set; }
    }
}
