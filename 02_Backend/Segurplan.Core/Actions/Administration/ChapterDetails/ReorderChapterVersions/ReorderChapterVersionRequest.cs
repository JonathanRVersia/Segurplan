using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.ReorderChapterVersions {
    public class ReorderChapterVersionRequest : IRequest<IRequestResponse<ReorderChapterVersionResponse>> {
        public int ChapterId { get; set; }

        public int ChapterVersionId { get; set; }
    }
}
