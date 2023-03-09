using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.ChapterDetails.ReorderChapterVersions;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.DeleteVersion {
    public class DeleteVersionRequestHandler : IRequestHandler<DeleteVersionRequest, IRequestResponse<DeleteVersionResponse>> {

        private readonly SegurplanContext context;
        private readonly IMediator mediator;

        public DeleteVersionRequestHandler(SegurplanContext context, IMediator mediator) {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<IRequestResponse<DeleteVersionResponse>> Handle(DeleteVersionRequest request, CancellationToken cancellationToken) {
            var subchapterVersions = await context.SubChapterVersion.Where(scv => scv.IdChapterVersion == request.VersionId).Include(scv => scv.IdSubChapterNavigation).ToListAsync();

            if (subchapterVersions.Any()) {
                var subchapterVersionsIds = subchapterVersions.Select(x => x.Id);
                var activityVersions = await context.ActivityVersion.Where(av => subchapterVersionsIds.Contains(av.IdSubChapterVersion)).Include(av => av.IdActivityNavigation).ToListAsync();

                context.SubChapter.RemoveRange(subchapterVersions.Select(x => x.IdSubChapterNavigation));
                context.SubChapterVersion.RemoveRange(subchapterVersions);

                if (activityVersions.Any()) {
                    foreach (var item in activityVersions) {
                        var relations = await context.ActivityRelations.Where(x => x.IdRelations == item.RelationsId).ToListAsync();
                        if (relations.Count > 0)
                            context.ActivityRelations.RemoveRange(relations);
                    }
                    var activitiesIds = activityVersions.Select(x => x.IdActivity);
                    var riskAndPreventiveMeasure = await context.RisksAndPreventiveMeasures.Where(x => activitiesIds.Contains(x.ActivityId)).ToListAsync();
                    var riskAndPreventiveMeasureIds = riskAndPreventiveMeasure.Select(x => x.Id);
                    var riskAndPreventiveMeasureMeasure = await context.RiskAndPreventiveMeasuresMeasures.Where(x => riskAndPreventiveMeasureIds.Contains(x.RisksAndPreventiveMeasuresId)).ToListAsync();
                    if (riskAndPreventiveMeasureMeasure.Count > 0)
                        context.RiskAndPreventiveMeasuresMeasures.RemoveRange(riskAndPreventiveMeasureMeasure);
                    if (riskAndPreventiveMeasure.Count > 0)
                        context.RisksAndPreventiveMeasures.RemoveRange(riskAndPreventiveMeasure);
                    context.Activity.RemoveRange(activityVersions.Select(x => x.IdActivityNavigation));
                    context.ActivityVersion.RemoveRange(activityVersions);
                }
            };
            var chapterVersion =  await context.ChapterVersion.Where(cv => cv.Id == request.VersionId).FirstOrDefaultAsync();
            int chapterId = chapterVersion.IdChapter;
            context.ChapterVersion.Remove(chapterVersion);
            var scheduledJob = await context.Job.Where(x => x.Arguments.Contains(chapterId.ToString()) && x.StateName == "Scheduled").FirstOrDefaultAsync();
            if (scheduledJob!=null && chapterVersion.ApprovementDate > DateTime.Now) {
                context.Job.Remove(scheduledJob);
            }
            int changes = await context.SaveChangesAsync();

            await mediator.Send(new ReorderChapterVersionRequest { ChapterId = chapterId });

            return changes > 0 ? RequestResponse.Ok<DeleteVersionResponse>()
                    : RequestResponse.NotOk<DeleteVersionResponse>();
        }
    }
}
