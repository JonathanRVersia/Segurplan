using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.ActivityDetails.DeleteCheck;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.DeleteCheck {
    public class DeleteCheckSubChapterRequestHandler : IRequestHandler<DeleteCheckSubChapterRequest, IRequestResponse<DeleteCheckSubChapterResponse>> {

        private readonly SegurplanContext context;
        private readonly IMediator mediator;

        public DeleteCheckSubChapterRequestHandler(SegurplanContext context, IMediator mediator) {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<IRequestResponse<DeleteCheckSubChapterResponse>> Handle(DeleteCheckSubChapterRequest request, CancellationToken cancellationToken) {

            var response = new DeleteCheckSubChapterResponse();

            IQueryable<RisksAndPreventiveMeasures> queryable = context.RisksAndPreventiveMeasures;

            queryable = AddWhereClause(queryable, request);

            response.RiskPreventiveSubChapterIds = await queryable.Select(rpm => rpm.SubChapterId).ToListAsync();

            if (!response.RiskPreventiveSubChapterIds.Any()) {
                var checkActivityPlansResponse = await mediator.Send(new DeleteCheckActivityRequest {
                    SubChapterIds = request.SubChapterIds == null ?
                                    new List<int> { request.SubChapterId } :
                                    request.SubChapterIds
                });

                response.ActivityHasPlansOrPreventiveMeasures = checkActivityPlansResponse.Value.ActivityHasPlansOrPreventiveMeasures;
            }

            return response.RiskPreventiveSubChapterIds.Any() || response.ActivityHasPlansOrPreventiveMeasures == true ? RequestResponse.Ok(response)
                                             : RequestResponse.NotFound<DeleteCheckSubChapterResponse>();
        }

        private IQueryable<RisksAndPreventiveMeasures> AddWhereClause(IQueryable<RisksAndPreventiveMeasures> queryable, DeleteCheckSubChapterRequest request) {

            if (request.SubChapterId != 0) {
                queryable = queryable.Where(rpm => rpm.SubChapterId == request.SubChapterId);
            } else if (request.SubChapterIds != null) {
                queryable = queryable.Where(rpm => request.SubChapterIds.Contains(rpm.SubChapterId));
            }

            return queryable;
        }
    }
}
