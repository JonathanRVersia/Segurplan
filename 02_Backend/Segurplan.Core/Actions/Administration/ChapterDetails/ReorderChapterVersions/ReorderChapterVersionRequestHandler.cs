using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.ReorderChapterVersions {
    public class ReorderChapterVersionRequestHandler : IRequestHandler<ReorderChapterVersionRequest, IRequestResponse<ReorderChapterVersionResponse>> {

        private readonly SegurplanContext context;
        private readonly IHttpContextAccessor contextAccessor;

        public ReorderChapterVersionRequestHandler(SegurplanContext context, IHttpContextAccessor contextAccessor) {
            this.context = context;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IRequestResponse<ReorderChapterVersionResponse>> Handle(ReorderChapterVersionRequest request, CancellationToken cancellationToken) {
            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var chapterVersions = await context.ChapterVersion.Where(ch => ch.IdChapter == request.ChapterId).ToListAsync();
            chapterVersions = chapterVersions.OrderBy(x => x.ApprovementDate).OrderByDescending(x => x.ApprovementDate.HasValue).ToList();

            int versionNumber = 1;
            bool isUpdated;
            var subChapterVersionWithValue = new List<SubChapterVersion>();
            
            foreach (var chapterVersion in chapterVersions) {
                var subChapterVersion = await context.SubChapterVersion.Where(ch => ch.IdChapterVersion == chapterVersion.Id).ToListAsync();

                if (subChapterVersion.Any() &&  chapterVersion.Id != request.ChapterVersionId) { 
                    if (chapterVersion.ApprovementDate < DateTime.Now && (chapterVersion.EndDate == null || chapterVersion.EndDate > DateTime.Now)) {
                        subChapterVersionWithValue = subChapterVersion;
                    }
                }

                isUpdated = false;
                var endDate = chapterVersions.Count() != versionNumber ? chapterVersions[versionNumber].ApprovementDate : null;

                if (chapterVersion.VersionNumber != versionNumber) {
                    chapterVersion.VersionNumber = versionNumber;
                    isUpdated = true;
                }

                if (chapterVersion.EndDate != endDate) {
                    chapterVersion.EndDate = endDate;
                    isUpdated = true;
                }

                if (isUpdated) {
                    chapterVersion.ModifiedBy = userId;
                    chapterVersion.UpdateDate = DateTime.Now;
                }

                versionNumber++;
            }

            var standingChapterVersion = chapterVersions.Where(x => x.ApprovementDate < DateTime.Now && x.EndDate == null || x.EndDate > DateTime.Now).FirstOrDefault();
            var standingSubChapterVersion = await context.SubChapterVersion.Where(ch => ch.IdChapterVersion == standingChapterVersion.Id).ToListAsync();

            context.UpdateRange(chapterVersions);

            await context.SaveChangesAsync();
        
            if (subChapterVersionWithValue.Any() && standingChapterVersion.Id != request.ChapterVersionId) {
                if (!standingSubChapterVersion.Any()) {
                    foreach (var subChapterVersion in subChapterVersionWithValue) {
                        subChapterVersion.IdChapterVersion = standingChapterVersion.Id;
                    }
                }    
            }

            context.UpdateRange(subChapterVersionWithValue);

            await context.SaveChangesAsync();

            return RequestResponse.Ok<ReorderChapterVersionResponse>();
        }
    }
}
