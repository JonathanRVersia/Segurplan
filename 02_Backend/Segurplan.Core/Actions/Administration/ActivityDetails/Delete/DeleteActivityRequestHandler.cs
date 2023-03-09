using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Delete {
    public class DeleteActivityRequestHandler : IRequestHandler<DeleteActivityRequest, IRequestResponse<DeleteActivityResponse>> {

        private readonly SegurplanContext context;

        public DeleteActivityRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<DeleteActivityResponse>> Handle(DeleteActivityRequest request, CancellationToken cancellationToken) {
            var activity = await context.Activity.Where(x => x.Id == request.ActivityId).Include(x => x.ActivityVersion).ToListAsync();

            var activityVersions = activity.SelectMany(x => x.ActivityVersion);
            foreach (var item in activityVersions) {
                var relations = await context.ActivityRelations.Where(x => x.IdRelations == item.RelationsId).ToListAsync();
                if(relations.Count>0)
                context.ActivityRelations.RemoveRange(relations);
            }
            context.ActivityVersion.RemoveRange(activityVersions);

            context.Activity.RemoveRange(activity);

            int changes = await context.SaveChangesAsync();

            return changes > 0 ? RequestResponse.Ok<DeleteActivityResponse>()
                               : RequestResponse.NotOk<DeleteActivityResponse>();
        }
    }
}
