using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Files.Download {
    public class DownloadAnagramRequestHandler : IRequestHandler<DownloadAnagramRequest, IRequestResponse<DownloadAnagramResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public DownloadAnagramRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<DownloadAnagramResponse>> Handle(DownloadAnagramRequest request, CancellationToken cancellationToken) {

            List<SafetyStudyPlanFile> files = new List<SafetyStudyPlanFile>();

            if (request.DefaultFile) {
                files = await context.DefaultSafetyStudyPlanFile.ProjectTo<SafetyStudyPlanFile>(mapper.ConfigurationProvider).ToListAsync();

            } else {
                files = await context.SafetyStudyPlanFile.Where(sf => request.FilesId.Any(x => x == sf.Id) && sf.IdSafetyStudyPlan == request.PlanId).ToListAsync();
            }

            return files.Any() ?
                RequestResponse.Ok(new DownloadAnagramResponse { Files = files }) :
                RequestResponse.NotFound<DownloadAnagramResponse>();
        }
    }
}
