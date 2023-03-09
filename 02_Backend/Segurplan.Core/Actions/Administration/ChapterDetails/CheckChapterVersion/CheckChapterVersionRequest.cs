using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.CheckChapterVersion {
    public class CheckChapterVersionRequest : IRequest<IRequestResponse<CheckChapterVersionResponse>> {
        public int PlanId { get; set; }
    }
}
