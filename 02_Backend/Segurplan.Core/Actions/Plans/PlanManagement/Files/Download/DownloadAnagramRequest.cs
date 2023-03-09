using System.Collections.Generic;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Files.Download {
    public class DownloadAnagramRequest : IRequest<IRequestResponse<DownloadAnagramResponse>> {
        public int PlanId { get; set; }

        public List<int> FilesId { get; set; }
        public bool DefaultFile { get; set; }
    }
}
