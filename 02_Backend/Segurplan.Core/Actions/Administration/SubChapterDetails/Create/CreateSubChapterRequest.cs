using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Create {
    public class CreateSubChapterRequest : IRequest<IRequestResponse<CreateSubChapterResponse>> {
        public string Title { get; set; }
        public int ChapterId { get; set; }
        public int ChapterVersionId { get; set; }
    }
}
