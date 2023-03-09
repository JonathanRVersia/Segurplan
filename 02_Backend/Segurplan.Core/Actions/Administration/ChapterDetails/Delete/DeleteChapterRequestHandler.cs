using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Delete {
    public class DeleteChapterRequestHandler : IRequestHandler<DeleteChapterRequest, IRequestResponse<DeleteChapterResponse>> {
        private readonly SegurplanContext context;

        public DeleteChapterRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<DeleteChapterResponse>> Handle(DeleteChapterRequest request, CancellationToken cancellationToken) {

            var subChapters = await context.SubChapter.Where(x => x.IdChapter == request.ChapterId).Include(x => x.SubChapterVersion).ToListAsync();

            var activities = await context.Activity.Where(x => subChapters.Select(y => y.Id).Contains(x.SubChapterId)).Include(x => x.ActivityVersion).ThenInclude(x => x.PlanActivityVersion).ToListAsync();

            var chapter = await context.Chapter.Where(x => x.Id == request.ChapterId).Include(x => x.ChapterVersion)/*.Include(x => x.IdVersionInfoNavigation)*/.FirstOrDefaultAsync();

            var activityVersions = activities.SelectMany(x => x.ActivityVersion);

            context.PlanActivityVersion.RemoveRange(activityVersions.SelectMany(x => x.PlanActivityVersion));

            context.ActivityVersion.RemoveRange(activityVersions);

            context.Activity.RemoveRange(activities);

            foreach (var item in activityVersions) {
                var relations = await context.ActivityRelations.Where(x => x.IdRelations == item.RelationsId).ToListAsync();
                if (relations.Count > 0)
                    context.ActivityRelations.RemoveRange(relations);
            }

            context.SubChapterVersion.RemoveRange(subChapters.SelectMany(x => x.SubChapterVersion));

            context.SubChapter.RemoveRange(subChapters);

            //Script chapters share ChapterVersionInfo with Id 1, created by app has his own chapterVersionInfo
            //if (chapter.IdVersionInfo != 1)
            //    context.ChapterVersionInfo.Remove(chapter.IdVersionInfoNavigation);

            context.ChapterVersion.RemoveRange(chapter.ChapterVersion);

            context.Chapter.Remove(chapter);

            int changes = await context.SaveChangesAsync();

            return changes > 0 ? RequestResponse.Ok<DeleteChapterResponse>()
                               : RequestResponse.NotOk<DeleteChapterResponse>();
        }
    }
}
