using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Delete {
    public class DeleteSubChapterRequestHandler : IRequestHandler<DeleteSubChapterRequest, IRequestResponse<DeleteSubChapterResponse>> {
        private readonly SegurplanContext context;

        public DeleteSubChapterRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<DeleteSubChapterResponse>> Handle(DeleteSubChapterRequest request, CancellationToken cancellationToken) {

            var activities = await context.Activity.Where(x => x.SubChapterId == request.SubChapterId).Include(x => x.ActivityVersion).ThenInclude(x=>x.PlanActivityVersion).ToListAsync();

            var activityVersions = activities.SelectMany(x => x.ActivityVersion);

            context.PlanActivityVersion.RemoveRange(activityVersions.SelectMany(x => x.PlanActivityVersion));
            foreach (var item in activityVersions) {
                var relations = await context.ActivityRelations.Where(x => x.IdRelations == item.RelationsId).ToListAsync();
                if (relations.Count > 0)
                    context.ActivityRelations.RemoveRange(relations);
            }
            context.ActivityVersion.RemoveRange(activityVersions);

            context.Activity.RemoveRange(activities);

            var subChapterVersions = await context.SubChapterVersion.Where(x => x.IdSubChapter == request.SubChapterId).ToListAsync();

            context.SubChapterVersion.RemoveRange(subChapterVersions);

            context.SubChapter.Remove(new SubChapter { Id = request.SubChapterId });

            int changes = await context.SaveChangesAsync();

            return changes > 0 ? RequestResponse.Ok<DeleteSubChapterResponse>()
                               : RequestResponse.NotOk<DeleteSubChapterResponse>();
        }
    }
}
