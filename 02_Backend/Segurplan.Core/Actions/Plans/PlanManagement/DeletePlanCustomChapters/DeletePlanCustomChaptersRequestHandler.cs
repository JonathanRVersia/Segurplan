using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.DeletePlanCustomChapters {
    public class DeletePlanCustomChaptersRequestHandler : IRequestHandler<DeletePlanCustomChaptersRequest, IRequestResponse> {

        private readonly SegurplanContext context;

        public DeletePlanCustomChaptersRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse> Handle(DeletePlanCustomChaptersRequest request, CancellationToken cancellationToken) {

            int changes = 0;

            var planActivitiesToDelete = await context.PlanActivityVersion.Where(x => x.IdPlan == request.Plan.Id && x.CustomActivityId != null).ToListAsync();

            var customSavecActivityIds = planActivitiesToDelete.Select(x => x.CustomActivityId).ToList();

            planActivitiesToDelete.RemoveAll(x => customSavecActivityIds.Contains(x.CustomActivityId));

            if (planActivitiesToDelete.Any()) {

                var customActivitiesToDeleteIds = planActivitiesToDelete.Select(x => x.CustomActivityId);

                var customActivities = await context.CustomActivity.Where(x => customActivitiesToDeleteIds.Contains(x.Id)).ToListAsync();

                var customSubChaptersToDeleteIds = new List<int>();
                var customChaptersToDelete = new List<CustomChapter>();
                var customSubChapters = new List<CustomSubchapter>();

                foreach (var customActivity in customActivities) {
                    if (customActivity.CustomSubchapterId != null)
                        customSubChaptersToDeleteIds.Add(customActivity.CustomSubchapterId ?? 0);
                }

                if (customSubChaptersToDeleteIds.Any()) {
                    customSubChapters = await context.CustomSubchapter.Where(x => customSubChaptersToDeleteIds.Contains(x.Id)).ToListAsync();

                    foreach (var customSubchapter in customSubChapters) {
                        if (customSubchapter.CustomChapterId != null)
                            customChaptersToDelete.Add(new CustomChapter { Id = customSubchapter.CustomChapterId ?? 0 });
                    }
                }

                if (customChaptersToDelete.Any())
                    context.RemoveRange(customChaptersToDelete);

                if (customSubChapters.Any())
                    context.RemoveRange(customSubChapters);

                if (customActivities.Any())
                    context.RemoveRange(customActivities);

                if (planActivitiesToDelete.Any())
                    context.RemoveRange(planActivitiesToDelete);

                changes = await context.SaveChangesAsync();
            }

            return changes > 0 ? RequestResponse.Ok() : RequestResponse.NotOk();
        }
    }
}
