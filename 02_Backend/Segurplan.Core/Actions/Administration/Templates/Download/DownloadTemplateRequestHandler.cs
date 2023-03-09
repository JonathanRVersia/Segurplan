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
    public class DownloadTemplateRequestHandler : IRequestHandler<DownloadTemplateRequest, IRequestResponse<DownloadTemplateResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public DownloadTemplateRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<DownloadTemplateResponse>> Handle(DownloadTemplateRequest request, CancellationToken cancellationToken) {
            var file = await context.Template.FirstOrDefaultAsync(sf => sf.Id == request.FileId);            

            return file != null ?
                RequestResponse.Ok(new DownloadTemplateResponse { File = file }) :
                RequestResponse.NotFound<DownloadTemplateResponse>();
        }
    }
}
