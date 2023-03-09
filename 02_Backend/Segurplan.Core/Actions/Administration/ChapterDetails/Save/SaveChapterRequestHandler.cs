using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.ChapterDetails.Models;
using Segurplan.Core.Actions.Administration.ChapterDetails.ReorderChapterVersions;
using Segurplan.Core.Actions.Administration.SubChapterDetails.Delete;
using Segurplan.Core.Database;
using Segurplan.Core.Domain.CacheServices;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Save {
    public class SaveChapterRequestHandler : IRequestHandler<SaveChapterRequest, IRequestResponse<SaveChapterResponse>> {

        private readonly IMapper mapper;
        private readonly SegurplanContext context;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IMediator mediator;
        private readonly ActivitiesCacheService activitiesCacheServices;
        private IBackgroundJobClient backgroundJobClient;


        public SaveChapterRequestHandler(IMapper mapper, SegurplanContext context, IHttpContextAccessor contextAccessor, IMediator mediator, ActivitiesCacheService activitiesCacheServices, IBackgroundJobClient backgroundJobClient) {
            this.mapper = mapper;
            this.context = context;
            this.contextAccessor = contextAccessor;
            this.mediator = mediator;
            this.activitiesCacheServices = activitiesCacheServices;
            this.backgroundJobClient = backgroundJobClient;
        }

        public async Task<IRequestResponse<SaveChapterResponse>> Handle(SaveChapterRequest request, CancellationToken cancellationToken) {

            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            bool isVersionReorderNeeded = false;
            var chapterVersion = new ChapterVersion();
            DateTime newCurrentApprovementDate;
            if (request.Id != 0) {
                if (!string.IsNullOrEmpty(request.RemoveSubChapterIds)) {
                    var subChapterIds = request.RemoveSubChapterIds.Split(",").Select(Int32.Parse).ToList();
                    foreach (var subChapterId in subChapterIds) {
                        await mediator.Send(new DeleteSubChapterRequest { SubChapterId = subChapterId });
                    }
                }

                chapterVersion = await context.ChapterVersion.Include(ch => ch.ProducedBy).FirstOrDefaultAsync(ch => ch.Id == request.Id);

                isVersionReorderNeeded = chapterVersion.ApprovementDate != request.ApprovementDate;
            }

            if(chapterVersion.ProducedBy != null && chapterVersion.ProducedBy.Any())
                DeleteProducedByList(chapterVersion.ProducedBy);

            var scheduledJob = await context.Job.Where(x => x.Arguments.Contains(chapterVersion.IdChapter.ToString()) && x.StateName == "Scheduled").FirstOrDefaultAsync();

            var isBorrador = chapterVersion.ApprovementDate == null || chapterVersion.ApprovementDate > DateTime.Now ? true : false;

            if (isBorrador && chapterVersion.ApprovementDate != request.ApprovementDate) {
               
                if(scheduledJob != null) {
                    context.Job.RemoveRange(scheduledJob);
                    await context.SaveChangesAsync();
                }

                if(request.ApprovementDate != null) {
                    newCurrentApprovementDate = (DateTime)request.ApprovementDate;
                    if (newCurrentApprovementDate <= DateTime.Today) {
                        backgroundJobClient.Enqueue(() => UpdateActRelationsToCurrentAsync(chapterVersion.IdChapter));
                    } else {
                        backgroundJobClient.Schedule(() => UpdateActRelationsToCurrentAsync(chapterVersion.IdChapter), newCurrentApprovementDate);
                    }
                }
            }

            chapterVersion = mapper.Map(request, chapterVersion);

            chapterVersion.ModifiedBy = userId;
            chapterVersion.UpdateDate = DateTime.Now;

            if (chapterVersion.Id == 0 && chapterVersion.IdChapter == 0)
                chapterVersion = CreateChapter(chapterVersion, userId);
            else if (chapterVersion.Id == 0) {
                CreateVersion(chapterVersion, userId);
            }

            if (chapterVersion.Id == 0)
                isVersionReorderNeeded = true;
            var multipleVersions = await context.ChapterVersion.Where(ch => ch.IdChapter == request.IdChapter).ToListAsync();
            if (chapterVersion.IdChapter == 0 || multipleVersions.Count<=1)
                isVersionReorderNeeded = false;
            context.Update(chapterVersion);

            int changes = await context.SaveChangesAsync();

            context.Entry(chapterVersion).State = EntityState.Detached;

            if (isVersionReorderNeeded)
                await mediator.Send(new ReorderChapterVersionRequest { ChapterId = request.IdChapter, ChapterVersionId = request.Id});

            activitiesCacheServices.Invalidate();

            return changes > 0 ? RequestResponse.Ok(new SaveChapterResponse(chapterVersion.Id, chapterVersion.IdChapter))
                               : RequestResponse.NotOk<SaveChapterResponse>();
        }

        private ChapterVersion CreateVersion(ChapterVersion chapterVersion, int userId) {
            chapterVersion.CreatedBy = userId;
            chapterVersion.CreateDate = DateTime.Now;
            return chapterVersion;
        }

        private ChapterVersion CreateChapter(ChapterVersion chapterVersion, int userId) {
            int number = (context.Chapter.Select(x => x.Number).Last()) + 1;

            chapterVersion.CreatedBy = userId;
            chapterVersion.CreateDate = DateTime.Now;
            chapterVersion.VersionNumber = 1;
            chapterVersion.Number = number;

            chapterVersion.IdChapterNavigation = new Chapter {
                Number = number,
                Title = chapterVersion.Title,
                CreatedBy = userId,
                ModifiedBy = userId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };

            return chapterVersion;
        }

        public async Task UpdateActRelationsToCurrentAsync(int chapterId) {

            //Traer todos los Subchapters y Activities de la version vigente
            var chapterVersionList = await context.ChapterVersion
                                         .Where(x => x.IdChapter == chapterId)
                                         .ToListAsync();

            var currentChapterVersion = chapterVersionList.Where(x => x.VersionNumber == chapterVersionList.Count - 1).FirstOrDefault();
            if (currentChapterVersion != null) {


                var subchapterVersionList = await context.SubChapterVersion.Where(x => x.IdChapterVersion == currentChapterVersion.Id).ToListAsync();
                var subchapterVersionIdList = subchapterVersionList.Select(x => x.Id).ToList();
                var subchapterIdList = subchapterVersionList.Select(x => x.IdSubChapter).ToList();

                var actVersionList = await context.ActivityVersion.Where(x => subchapterVersionIdList.Contains(x.IdSubChapterVersion)).ToListAsync();
                var actVersionIdList = actVersionList.Select(x => x.IdActivity).ToList();

                //Comprobar si se usan sus IDs en ActivityRelations
                var activityRelationsBySubchapt = await context.ActivityRelations
                    .Where(x => subchapterIdList.Contains((int)x.IdSubchapterRelation)).ToListAsync();

                var activityRelationsByAct = await context.ActivityRelations
                    .Where(x => actVersionIdList.Contains((int)x.IdActivityRelation)).ToListAsync();

                //Borrar las relaciones existentes que coincidan y sustituirlas por su equivalente de la nueva version
                var borradorChapterVersion = chapterVersionList.Where(x => x.VersionNumber == chapterVersionList.Count).FirstOrDefault();

                var borradorSubchapterVersionList = await context.SubChapterVersion.Where(x => x.IdChapterVersion == borradorChapterVersion.Id).ToListAsync();
                var borradorSubchapterVersionIdList = borradorSubchapterVersionList.Select(x => x.IdSubChapter).ToList();

                var borradorActivityVersionList = await context.ActivityVersion.Where(x => borradorSubchapterVersionIdList.Contains(x.IdSubChapterVersion)).ToListAsync();

                var newActRelationsBySubchapt = new List<ActivityRelations>();
                var newActRelationsByAct = new List<ActivityRelations>();
                //Para las relaciones por subchapter
                if (activityRelationsBySubchapt.Count > 0) {
                    foreach (var relation in activityRelationsBySubchapt) {
                        var number = subchapterVersionList.Where(x => x.IdSubChapter == relation.IdSubchapterRelation).Select(x => x.Number).FirstOrDefault();
                        var borradorSubchapterId = borradorSubchapterVersionList.Where(x => x.Number == number).Select(x => x.IdSubChapter).FirstOrDefault();

                        if (borradorSubchapterId != 0) {
                            var newRelation = new ActivityRelations() {
                                IdRelations = relation.IdRelations,
                                IdSubchapterRelation = borradorSubchapterId,
                                IdActivityRelation = relation.IdActivityRelation,
                                IdChapterRelation = relation.IdChapterRelation
                            };
                            newActRelationsBySubchapt.Add(newRelation);
                        }
                    }
                    context.ActivityRelations.RemoveRange(activityRelationsBySubchapt);
                    context.ActivityRelations.AddRange(newActRelationsBySubchapt);
                    await context.SaveChangesAsync();
                }

                //Para las relaciones por activity
                if (activityRelationsByAct.Count > 0) {
                    foreach (var relation in activityRelationsByAct) {

                        var actVersion = await context.ActivityVersion
                            .Where(x => x.IdActivity == relation.IdActivityRelation).FirstOrDefaultAsync();

                        var subchapterNumber = await context.SubChapterVersion
                            .Where(x => x.Id == actVersion.IdSubChapterVersion).Select(x => x.Number).FirstOrDefaultAsync();

                        var borradorSubchapterId = borradorSubchapterVersionList.Where(x => x.Number == subchapterNumber).Select(x => x.IdSubChapter).FirstOrDefault();

                        var borradorActVersion = borradorActivityVersionList.Where(x => x.IdSubChapterVersion == borradorSubchapterId && x.Number == actVersion.Number).FirstOrDefault();

                        if (borradorActVersion != null) {
                            var newRelation = new ActivityRelations() {
                                IdRelations = relation.IdRelations,
                                IdSubchapterRelation = relation.IdSubchapterRelation,
                                IdActivityRelation = borradorActVersion.IdActivity,
                                IdChapterRelation = relation.IdChapterRelation
                            };
                            newActRelationsByAct.Add(newRelation);
                        }
                    }
                    context.ActivityRelations.RemoveRange(activityRelationsByAct);
                    context.ActivityRelations.AddRange(newActRelationsByAct);
                    await context.SaveChangesAsync();
                }
            }
        }

        private void DeleteProducedByList(List<UserChapterVersion> dbProducedBy) => context.UserChapterVersion.RemoveRange(dbProducedBy);
    }
}
