using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.DeleteVersion {
    public class DeleteVersionRequest : IRequest<IRequestResponse<DeleteVersionResponse>> {
        public int VersionId { get; set; }
    }
}
