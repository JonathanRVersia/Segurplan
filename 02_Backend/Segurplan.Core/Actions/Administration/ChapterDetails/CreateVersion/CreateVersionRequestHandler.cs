using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.ChapterDetails.Models;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.CreateVersion {
    public class CreateVersionRequestHandler : IRequestHandler<CreateVersionRequest, IRequestResponse<CreateVersionResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;

        public CreateVersionRequestHandler(SegurplanContext context, IMapper mapper, IHttpContextAccessor contextAccessor) {
            this.context = context;
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IRequestResponse<CreateVersionResponse>> Handle(CreateVersionRequest request, CancellationToken cancellationToken) {

            var chapter = await context.Chapter.Where(ch => ch.Id == request.ChapterId).Include(ch => ch.ChapterVersion).ThenInclude(chv => chv.ProducedBy).FirstOrDefaultAsync();

            var chapterVersion = await CreateChapterVersion(chapter);

            //context.Add(chapterVersion);

            //await context.SaveChangesAsync();

            chapterVersion.IdChapterNavigation = chapter;
            await DuplicateExistingRiskAssignmentToNewBorrador(request.ChapterId);
            return RequestResponse.Ok(new CreateVersionResponse(mapper.Map<ChapterDetailsChapterVersion>(chapterVersion)));
        }

        private async Task<ChapterVersion> CreateChapterVersion(Chapter chapter) {
            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var currentChapterVersion = chapter.ChapterVersion
                .Where(x => x.ApprovementDate != null ?
                    x.ApprovementDate < DateTime.Now && x.EndDate == null || x.EndDate > DateTime.Now :
                    x.VersionNumber == chapter.ChapterVersion.Max(y => y.VersionNumber))
                        .FirstOrDefault();

            var newChapterVersion = mapper.Map<NewChapterVersion>(currentChapterVersion);
            newChapterVersion.CreatedBy = userId;
            newChapterVersion.ModifiedBy = userId;

            var currentSubChapterVersions = await context.SubChapterVersion.Where(x => x.IdChapterVersion == currentChapterVersion.Id).Include(x => x.IdSubChapterNavigation).ToListAsync();

            if (currentSubChapterVersions.Any())
                currentSubChapterVersions = currentSubChapterVersions.OrderBy(x => x.Number).ToList();

            List<ActivityRelations> newActivityRelations = new List<ActivityRelations>();

            var existingRelations = await context.ActivityRelations.ToListAsync();

            foreach (var currentSubChapterVersion in currentSubChapterVersions) {

                var newSubChapterVersion = mapper.Map<NewSubChapterVersion>(currentSubChapterVersion);
                newSubChapterVersion.CreatedBy = userId;
                newSubChapterVersion.ModifiedBy = userId;
                newSubChapterVersion.IdSubChapterNavigation.CreatedBy = userId;
                newSubChapterVersion.IdSubChapterNavigation.ModifiedBy = userId;

                var currentActivityVersions = await context.ActivityVersion.Where(x => x.IdSubChapterVersion == currentSubChapterVersion.Id).Include(x => x.IdActivityNavigation).ToListAsync();

                if (currentActivityVersions.Any())
                    currentActivityVersions = currentActivityVersions.OrderBy(x => x.Number).ToList();

                foreach (var currentActivityVersion in currentActivityVersions) {

                    var newActivityVersion = mapper.Map<NewActivityVersion>(currentActivityVersion);
                    newActivityVersion.CreatedBy = userId;
                    newActivityVersion.ModifiedBy = userId;
                    newActivityVersion.IdActivityNavigation.CreatedBy = userId;
                    newActivityVersion.IdActivityNavigation.ModifiedBy = userId;
                    newActivityVersion.IdActivityNavigation.SubChapter = newSubChapterVersion.IdSubChapterNavigation;
                    
                    if(newActivityVersion.RelationsId != null) {
                        int nextId = existingRelations.Select(x => x.IdRelations).Max() + 1;
                        foreach (var relation in existingRelations.Where(x => x.IdRelations == newActivityVersion.RelationsId).ToList()) {
                            relation.IdRelations = nextId;
                            relation.Id = 0;
                            existingRelations.Add(relation);
                            context.Add(relation);
                        }
                        newActivityVersion.RelationsId = nextId;
                    }
                    newSubChapterVersion.ActivityVersion.Add(newActivityVersion);
                }

                newChapterVersion.SubChapterVersion.Add(newSubChapterVersion);
            }

            var chapterVersion = mapper.Map<ChapterVersion>(newChapterVersion);
            
            context.Update(chapterVersion);
            var changes = await context.SaveChangesAsync();

            return chapterVersion;
        }
        public async Task DuplicateExistingRiskAssignmentToNewBorrador(int chapterId) {
            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var chapterVersionList = await context.ChapterVersion
                                         .Where(x => x.IdChapter == chapterId)
                                         .ToListAsync().ConfigureAwait(true);
            var currentChapterVersion = chapterVersionList.Where(x => x.VersionNumber == chapterVersionList.Count - 1).FirstOrDefault();
            if (currentChapterVersion != null) {

                var subchapterVersionList = await context.SubChapterVersion.Where(x => x.IdChapterVersion == currentChapterVersion.Id).ToListAsync().ConfigureAwait(true);
                var subchapterVersionIdList = subchapterVersionList.Select(x => x.Id).ToList();

                var actVersionList = await context.ActivityVersion.Where(x => subchapterVersionIdList.Contains(x.IdSubChapterVersion)).ToListAsync().ConfigureAwait(true);
                var actVersionIdList = actVersionList.Select(x => x.IdActivity).ToList();

                var borradorChapterVersion = chapterVersionList.Where(x => x.VersionNumber == chapterVersionList.Count).FirstOrDefault();

                var borradorSubchapterVersionList = await context.SubChapterVersion.Where(x => x.IdChapterVersion == borradorChapterVersion.Id).ToListAsync().ConfigureAwait(true);
                var borradorSubchapterVersionIdList = borradorSubchapterVersionList.Select(x => x.IdSubChapter).ToList();

                var borradorActivityVersionList = await context.ActivityVersion.Where(x => borradorSubchapterVersionIdList.Contains(x.IdSubChapterVersion)).ToListAsync().ConfigureAwait(true);

                var RiskAsignmentsCurrentChapterVersion = await context.RisksAndPreventiveMeasures
                .Where(x => actVersionIdList.Contains(x.ActivityId)).ToListAsync().ConfigureAwait(true);
                var RiskAsignmentsCurrentChapterVersionIds = RiskAsignmentsCurrentChapterVersion.Select(x => x.Id).ToList();
                var RiskAndPreventiveMeasuresAssignment = await context.RiskAndPreventiveMeasuresMeasures.Where(x => RiskAsignmentsCurrentChapterVersionIds.Contains(x.RisksAndPreventiveMeasuresId)).ToListAsync();
                foreach (var riskAssignment in RiskAsignmentsCurrentChapterVersion) {
                    var numberSubchap = subchapterVersionList.Where(x => x.IdSubChapter == riskAssignment.SubChapterId).Select(x => x.Number).FirstOrDefault();
                    var numberAct = actVersionList.Where(x => x.IdActivity == riskAssignment.ActivityId).Select(x => x.Number).FirstOrDefault();

                    var newSubchapterId = borradorSubchapterVersionList.Where(x => x.Number == numberSubchap).Select(x => x.IdSubChapter).FirstOrDefault();
                    var newSubchapterVersionId = borradorSubchapterVersionList.Where(x => x.Number == numberSubchap).Select(x => x.Id).FirstOrDefault();
                    var newActivityId = borradorActivityVersionList.Where(x => x.IdSubChapterVersion == newSubchapterVersionId && x.Number == numberAct).Select(x => x.IdActivity).FirstOrDefault();
                    if (newSubchapterId != 0 && newActivityId != 0) {
                        riskAssignment.ActivityId = newActivityId;
                        riskAssignment.SubChapterId = newSubchapterId;
                        riskAssignment.CreateDate = DateTime.Now;
                        riskAssignment.UpdateDate = DateTime.Now;
                        riskAssignment.CreatedBy = userId;
                        riskAssignment.CreatedBy = userId;
                        var RiskAndPreventiveMeasureMeasures = RiskAndPreventiveMeasuresAssignment.Where(x=>x.RisksAndPreventiveMeasuresId==riskAssignment.Id).ToList();
                        riskAssignment.Id = 0;
                        foreach (var riskAndPrevMeasureMeasure in RiskAndPreventiveMeasureMeasures) {
                            context.RiskAndPreventiveMeasuresMeasures.Add(new RiskAndPreventiveMeasuresMeasures { RisksAndPreventiveMeasures = riskAssignment , PreventiveMeasureId= riskAndPrevMeasureMeasure.PreventiveMeasureId, PreventiveMeasureOrder = riskAndPrevMeasureMeasure.PreventiveMeasureOrder});
                        }
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
