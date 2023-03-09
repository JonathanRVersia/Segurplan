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

namespace Segurplan.Core.Actions.Plans.PlanManagement.Files.View {
    public class ViewPlanPlaneFileRequestHandler : IRequestHandler<ViewPlanPlaneFileRequest, IRequestResponse<ViewPlanPlaneFileResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public ViewPlanPlaneFileRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<ViewPlanPlaneFileResponse>> Handle(ViewPlanPlaneFileRequest request, CancellationToken cancellationToken) {

            var result = new List<ViewPlanPlaneItem>();

            if (request.IsFromGenericBlueprint) {
                if (request.GenericPlaneId.HasValue) {
                    result = await context.Plane
                    .Where(file => file.Id == request.GenericPlaneId)
                    .ProjectTo<ViewPlanPlaneItem>(mapper.ConfigurationProvider)
                    .ToListAsync();
                }
            } else {
                result = await context.SafetyStudyPlanPlaneFile
                       .Where(file => file.SafetyStudyPlanPlaneId == request.CreatedPlanId)
                       .ProjectTo<ViewPlanPlaneItem>(mapper.ConfigurationProvider)
                       .ToListAsync();
            }


            return result.Any() ?
                RequestResponse.Ok(new ViewPlanPlaneFileResponse(result)) :
                RequestResponse.NotFound<ViewPlanPlaneFileResponse>();
        }
    }
}
