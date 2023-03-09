using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.ChapterDetails.DeleteCheck;
using Segurplan.Core.Actions.Administration.ChapterDetails.ReorderChapterVersions;
using Segurplan.Core.Actions.Administration.SubChapterDetails.DeleteCheck;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.DeleteChapterVersionCheck {
    public class DeleteChapterVersionCheckRequestHandler : IRequestHandler<DeleteChapterVersionCheckRequest, IRequestResponse<DeleteCheckChapterResponse>> {

        private readonly SegurplanContext context;
        private readonly IMediator mediator;

        public DeleteChapterVersionCheckRequestHandler(SegurplanContext context, IMediator mediator) {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<IRequestResponse<DeleteCheckChapterResponse>> Handle(DeleteChapterVersionCheckRequest request, CancellationToken cancellationToken) {
            var response = new DeleteCheckChapterResponse { SubChapterRiskPreventiveIds = new List<int>() };

            var chapterVersion = await context.ChapterVersion.Where(x => x.Id == request.ChapterVersionId)
                .FirstOrDefaultAsync();

            if (chapterVersion.ApprovementDate == null || chapterVersion.ApprovementDate > DateTime.Now) {
                return RequestResponse.NotFound(response);
            } else {

                var subChapterIds = await context.SubChapterVersion.Where(x => x.IdChapterVersion == request.ChapterVersionId).Select(x => x.IdSubChapter).ToListAsync();

                if (subChapterIds.Any()) {
                    var deleteSubchapterCheckResponse = await mediator.Send(new DeleteCheckSubChapterRequest { SubChapterIds = subChapterIds });

                    if (deleteSubchapterCheckResponse.Status == RequestStatus.Ok) {
                        response.SubChapterRiskPreventiveIds = deleteSubchapterCheckResponse.Value.RiskPreventiveSubChapterIds;
                        response.ActivityHasPlansOrPreventiveMeasures = deleteSubchapterCheckResponse.Value.ActivityHasPlansOrPreventiveMeasures;
                    }
                }

                return response.SubChapterRiskPreventiveIds.Any() || response.ChapterRiskPreventiveId != null || response.ActivityHasPlansOrPreventiveMeasures
                               ? RequestResponse.Ok(response)
                               : RequestResponse.NotFound(response);
            }
        }
    }
}
