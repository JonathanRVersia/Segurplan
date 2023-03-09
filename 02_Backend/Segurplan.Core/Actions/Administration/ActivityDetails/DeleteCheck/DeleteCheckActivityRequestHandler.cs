using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.DeleteCheck {
    public class DeleteCheckActivityRequestHandler : IRequestHandler<DeleteCheckActivityRequest, IRequestResponse<DeleteCheckActivityResponse>> {

        private readonly SegurplanContext context;

        public DeleteCheckActivityRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<DeleteCheckActivityResponse>> Handle(DeleteCheckActivityRequest request, CancellationToken cancellationToken) {

            IQueryable<ActivityVersion> queryable = context.ActivityVersion;

            queryable = AddWhereClause(queryable, request);

            bool hasPlansOrPreventiveMeasures = await queryable.AnyAsync(x => x.PlanActivityVersion.Any());

            if (!hasPlansOrPreventiveMeasures) {
                var activityIds = await queryable.Select(x => x.IdActivity).ToListAsync();

                hasPlansOrPreventiveMeasures = await context.RisksAndPreventiveMeasures.AnyAsync(x => activityIds.Contains(x.ActivityId));
            }

            return hasPlansOrPreventiveMeasures==true?RequestResponse.Ok(new DeleteCheckActivityResponse { ActivityHasPlansOrPreventiveMeasures = hasPlansOrPreventiveMeasures })
                                :RequestResponse.NotFound(new DeleteCheckActivityResponse { ActivityHasPlansOrPreventiveMeasures = hasPlansOrPreventiveMeasures });
        }

        private IQueryable<ActivityVersion> AddWhereClause(IQueryable<ActivityVersion> queryable, DeleteCheckActivityRequest request) {

            if (request.ActivityId != 0)
                queryable = queryable.Where(av => av.IdActivity == request.ActivityId);
            else if (request.ActivityIds != null)
                queryable = queryable.Where(av => request.ActivityIds.Contains(av.IdActivity));
            else if (request.SubChapterIds != null)//Used from DeleteSubChapterRequestHandler
                queryable = queryable.Where(av => context.Activity.Where(ac => request.SubChapterIds.Contains(ac.SubChapterId)).Select(x => x.Id).Contains(av.IdActivity));

            return queryable;
        }
    }
}
