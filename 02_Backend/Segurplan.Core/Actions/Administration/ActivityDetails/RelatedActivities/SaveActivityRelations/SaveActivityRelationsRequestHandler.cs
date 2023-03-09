using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;
using Segurplan.Core.Actions.Administration.ActivityDetails;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;
using AutoMapper;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.RelatedActivities.SaveActivityRelations {
    public class SaveActivityRelationsRequestHandler : IRequestHandler<SaveActivityRelationsRequest, IRequestResponse<SaveActivityRelationsResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public SaveActivityRelationsRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<SaveActivityRelationsResponse>> Handle(SaveActivityRelationsRequest request, CancellationToken cancellationToken) {
            var activityUpdate = await context.ActivityVersion.Where(z => z.Id == request.ActivityId).FirstOrDefaultAsync();
            if (request.RelationsId == null && request.RelationsDataList.Count>0) {
                var newRelationsId = await context.ActivityVersion.MaxAsync(z => z.RelationsId) ?? 0;
                activityUpdate.RelationsId = newRelationsId + 1;
                context.Entry(activityUpdate).Property(p=>p.RelationsId).IsModified=true;
                //await context.SaveChangesAsync();
            }
            var currentData = context.ActivityRelations.Where(c => c.IdRelations == activityUpdate.RelationsId);
            if(currentData.Count()>0)
            context.ActivityRelations.RemoveRange(currentData);
            var addRelation = mapper.Map<List<ActivityRelations>>(request.RelationsDataList);
            foreach (var item in addRelation) {
                item.IdRelations = activityUpdate.RelationsId ?? default(int);
            }
            context.ActivityRelations.AddRange(addRelation);
            await context.SaveChangesAsync();
            return RequestResponse.Ok(new SaveActivityRelationsResponse());
        }
    }
}
