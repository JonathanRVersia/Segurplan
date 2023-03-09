using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.ActivityDetails.DeleteCheck;
using Segurplan.Core.Actions.Administration.SubChapterDetails.DeleteCheck;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.DeleteCheck {

    public class DeleteCheckChapterRequestHandler : IRequestHandler<DeleteCheckChapterRequest, IRequestResponse<DeleteCheckChapterResponse>> {

        private readonly SegurplanContext context;
        private readonly IMediator mediator;

        public DeleteCheckChapterRequestHandler(SegurplanContext context, IMediator mediator) {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<IRequestResponse<DeleteCheckChapterResponse>> Handle(DeleteCheckChapterRequest request, CancellationToken cancellationToken) {
            var response = new DeleteCheckChapterResponse { SubChapterRiskPreventiveIds=new List<int>()};

            if (await context.RisksAndPreventiveMeasures.Where(rpm => rpm.ChapterId == request.ChapterId).AnyAsync())
                response.ChapterRiskPreventiveId = request.ChapterId;

            //Cuando solo se puedan elegir los subcapitulos relativos a este capitulo en RiskPreventiveMeasures no hará falta
            if (response.ChapterRiskPreventiveId == null) {
                var subChapterIds = await context.SubChapter.Where(sc => sc.IdChapter == request.ChapterId).Select(x => x.Id).ToListAsync();

                if (subChapterIds.Any()) {
                    var deleteSubchapterCheckResponse = await mediator.Send(new DeleteCheckSubChapterRequest { SubChapterIds = subChapterIds });

                    if (deleteSubchapterCheckResponse.Status == RequestStatus.Ok) {
                        response.SubChapterRiskPreventiveIds = deleteSubchapterCheckResponse.Value.RiskPreventiveSubChapterIds;
                        response.ActivityHasPlansOrPreventiveMeasures = deleteSubchapterCheckResponse.Value.ActivityHasPlansOrPreventiveMeasures;
                    }
                }
            } else {
                var subChapterIds = await context.SubChapter.Where(sc => sc.IdChapter == request.ChapterId).Select(x => x.Id).ToListAsync();

                if (subChapterIds != null) {
                    var activityCheckResult = await mediator.Send(new DeleteCheckActivityRequest { SubChapterIds = subChapterIds });
                    response.ActivityHasPlansOrPreventiveMeasures = activityCheckResult.Value.ActivityHasPlansOrPreventiveMeasures;
                }

            }


            return response.SubChapterRiskPreventiveIds.Any() || response.ChapterRiskPreventiveId != null || response.ActivityHasPlansOrPreventiveMeasures
                            ? RequestResponse.Ok(response)
                            : RequestResponse.NotFound(response);
        }
    }
}
