using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.CreateVersion {
    public class CreateVersionRequest : IRequest<IRequestResponse<CreateVersionResponse>> {
        public int ChapterId { get; set; }
    }
}
