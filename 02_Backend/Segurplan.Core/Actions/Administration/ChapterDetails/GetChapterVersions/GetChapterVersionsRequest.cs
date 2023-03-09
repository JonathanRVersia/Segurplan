using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.GetChapterVersions {
    public class GetChapterVersionsRequest : IRequest<IRequestResponse<GetChapterVersionsResponse>> {
        public int ChapterId { get; set; }
    }
}
