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
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Details {
    public class ActivityDetailsRequestHandler : IRequestHandler<ActivityDetailsRequest, IRequestResponse<ActivityDetailsResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public ActivityDetailsRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<ActivityDetailsResponse>> Handle(ActivityDetailsRequest request, CancellationToken cancellationToken) {
            var activityVersion = await context.ActivityVersion.ProjectTo<ActivityDetailsActivityVersion>(mapper.ConfigurationProvider).Where(av => av.Id == request.ActivityVersionId).FirstOrDefaultAsync();

            if (activityVersion == null)
                return RequestResponse.NotFound<ActivityDetailsResponse>();

            return RequestResponse.Ok(new ActivityDetailsResponse(activityVersion));
        }
    }
}
