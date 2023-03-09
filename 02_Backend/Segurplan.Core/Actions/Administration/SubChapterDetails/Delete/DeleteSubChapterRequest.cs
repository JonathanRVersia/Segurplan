using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Delete {
    public class DeleteSubChapterRequest : IRequest<IRequestResponse<DeleteSubChapterResponse>> {
        public int SubChapterId { get; set; }
    }
}
