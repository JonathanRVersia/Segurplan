using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.GetLastChapterVersionId {
    public class GetLastChapterVersionIdRequest : IRequest<IRequestResponse<GetLastChapterVersionIdResponse>> {
        public int ChapterId { get; set; }
    }
}
