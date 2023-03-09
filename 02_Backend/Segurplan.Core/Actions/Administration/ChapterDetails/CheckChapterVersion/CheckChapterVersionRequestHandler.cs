using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.CheckChapterVersion {
    public class CheckChapterVersionRequestHandler : IRequestHandler<CheckChapterVersionRequest, IRequestResponse<CheckChapterVersionResponse>> {

        private readonly SegurplanContext context;
        private readonly IHttpContextAccessor contextAccessor;

        public CheckChapterVersionRequestHandler(SegurplanContext context, IHttpContextAccessor contextAccessor) {
            this.context = context;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IRequestResponse<CheckChapterVersionResponse>> Handle(CheckChapterVersionRequest request, CancellationToken cancellationToken) {

            var notValidChapterNumbers = new List<int>();

            var activityVersionIds = await context.PlanActivityVersion.Where(x => x.CustomActivityId == null && x.IdPlan == request.PlanId && x.ChapterPosition != 1).Select(x => x.IdActivityVersion).ToListAsync();

            if (activityVersionIds.Any()) {
                notValidChapterNumbers = await context.ActivityVersion
                    .Where(x => activityVersionIds.Contains(x.Id) &&
                    (x.IdSubChapterVersionNavigation.IdChapterVersionNavigation.EndDate < DateTime.Now ||
                    x.IdSubChapterVersionNavigation.IdChapterVersionNavigation.ApprovementDate > DateTime.Now
                    )).Select(x => x.IdSubChapterVersionNavigation.IdChapterVersionNavigation.IdChapterNavigation.Number).Distinct().ToListAsync();
            }

            return notValidChapterNumbers.Count > 0 ? RequestResponse.Ok(new CheckChapterVersionResponse(notValidChapterNumbers))
                               : RequestResponse.NotFound<CheckChapterVersionResponse>();
        }
    }
}
