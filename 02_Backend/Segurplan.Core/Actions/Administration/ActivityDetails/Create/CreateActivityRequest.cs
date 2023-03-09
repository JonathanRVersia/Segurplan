using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Create {
    public class CreateActivityRequest : IRequest<IRequestResponse<CreateActivityResponse>> {
        public string Title { get; set; }
        public int SubChapterId { get; set; }
        public int SubChapterVersionId { get; set; }
    }
}
