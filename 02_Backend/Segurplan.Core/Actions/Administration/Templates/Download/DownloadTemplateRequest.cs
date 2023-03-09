using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Files.Download {
    public class DownloadTemplateRequest : IRequest<IRequestResponse<DownloadTemplateResponse>> {
        public int PlanId { get; set; }

        public int FileId { get; set; }

        public bool DefaultFile { get; set; }
    }
}
