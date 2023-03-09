using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.UpdatePlanCustomChapters {
    public class UpdatePlanCustomChaptersRequestHandler : IRequestHandler<UpdatePlanCustomChaptersRequest, IRequestResponse<UpdatePlanCustomChaptersResponse>> {

        private readonly SegurplanContext context;
        private readonly IHttpContextAccessor contextAccessor;

        public UpdatePlanCustomChaptersRequestHandler(SegurplanContext context, IHttpContextAccessor contextAccessor) {
            this.context = context;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IRequestResponse<UpdatePlanCustomChaptersResponse>> Handle(UpdatePlanCustomChaptersRequest request, CancellationToken cancellationToken) {

            var customPlanActivities = new List<SelectedPlanActivity>();

            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            //Si hay alguna actividadCustom es obligatorio que se haya creado o Capitulo o SubCapitulo
            if (request.Chapters.SelectMany(chap => chap.SubChapter).Any(sChap => sChap.Activities.Any(act => act.IsCustomActivity && act.Id != 0))) {
                foreach (var chapter in request.Chapters) {

                    if (chapter.IsCustomChapter && chapter.Id != 0) {
                        await UpdateCustomChapter(chapter, userId, customPlanActivities);
                    } else {
                        foreach (var subChapter in chapter.SubChapter) {

                            if (subChapter.IsCustomSubChapter && subChapter.Id != 0) {
                                await UpdateCustomSubChapter(subChapter, userId, customPlanActivities, chapter);
                            } else {
                                foreach (var activity in subChapter.Activities) {

                                    if (activity.IsCustomActivity && activity.Id != 0) {
                                        await UpdateCustomActivity(activity, userId, customPlanActivities, chapter , subChapter);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            //if (request.Chapters.Any())
            //    await UpdateCustomChapters(request.Chapters, userId, customPlanActivities);

            //if (request.CustomSubChapters.Any())
            //    await UpdateCustomSubChapters(request.CustomSubChapters, userId, customPlanActivities);

            //if (request.CustomActivities.Any())
            //    await UpdateCustomActivities(request.CustomActivities, userId, customPlanActivities);






            //var customChapterIds = request.CustomChapters.Select(x => x.Id).ToList();

            //var chaptersToUpdate = context.CustomChapter.Where(chap => customChapterIds.Contains(chap.Id))
            //    .Include(chap=>chap.CustomSubchapters)
            //        .ThenInclude(sChap=>sChap.CustomActivities).ToList();

            //foreach (var chapterToUpdate in chaptersToUpdate) {
            //    var requestChapter = request.CustomChapters.Where(chap => chap.Id == chapterToUpdate.Id).FirstOrDefault();

            //    chapterToUpdate.Title = requestChapter.Title;
            //    chapterToUpdate.ModifiedBy = userId;
            //    chapterToUpdate.UpdateDate = DateTime.Now;

            //    foreach (var subChapterToUpdate in chapterToUpdate.CustomSubchapters) {
            //        var requestSubChapter = requestChapter.SubChapter.Where(sChap => sChap.Id == subChapterToUpdate.Id).FirstOrDefault();

            //        subChapterToUpdate.Title = requestSubChapter.Title;
            //        subChapterToUpdate.ModifiedBy = userId;
            //        subChapterToUpdate.UpdateDate = DateTime.Now;

            //        foreach (var activityToUpdate in subChapterToUpdate.CustomActivities) {
            //            var requestActivity = requestSubChapter.Activities.Where(act => act.Id == activityToUpdate.Id).FirstOrDefault();

            //            activityToUpdate.Title = requestActivity.Title;
            //            activityToUpdate.ModifiedBy = userId;
            //            activityToUpdate.UpdateDate = DateTime.Now;

            //            customPlanActivities.Add(CreateCustomPlanActivity(requestChapter, requestSubChapter, requestActivity,activityToUpdate.Id));
            //        }
            //    }
            //}

            //context.UpdateRange(chaptersToUpdate);
            //int changes = await context.SaveChangesAsync();

            return customPlanActivities.Count() > 0 ? RequestResponse.Ok(new UpdatePlanCustomChaptersResponse(customPlanActivities))
                : RequestResponse.NotOk<UpdatePlanCustomChaptersResponse>();
        }

        private async Task UpdateCustomActivity(PlanActivity planActivity, int userId, List<SelectedPlanActivity> customPlanActivities, PlanChapter chapter, PlanSubChapter subChapter) {

            var activityToUpdate = await context.CustomActivity.Where(act => act.Id == planActivity.Id)
                    .FirstOrDefaultAsync();


            //activityToUpdate.Title = planActivity.Title;
            activityToUpdate.Description = planActivity.Description;
            activityToUpdate.ModifiedBy = userId;
            activityToUpdate.UpdateDate = DateTime.Now;

            customPlanActivities.Add(CreateCustomPlanActivity(chapter, subChapter, planActivity, activityToUpdate.Id));

            context.Update(activityToUpdate);
            await context.SaveChangesAsync();
        }

        private async Task UpdateCustomActivities(List<PlanActivity> planActivites, int userId, List<SelectedPlanActivity> customPlanActivities) {
            var customActivitiesIds = planActivites.Select(x => x.Id).ToList();

            var activitiesToUpdate = await context.CustomActivity.Where(act => customActivitiesIds.Contains(act.Id))
                    .ToListAsync();

            foreach (var activityToUpdate in activitiesToUpdate) {
                var requestActivity = planActivites.Where(act => act.Id == activityToUpdate.Id).FirstOrDefault();

                //activityToUpdate.Title = requestActivity.Title;
                activityToUpdate.Description = requestActivity.Description;
                activityToUpdate.ModifiedBy = userId;
                activityToUpdate.UpdateDate = DateTime.Now;

                customPlanActivities.Add(CreateCustomPlanActivity(new PlanChapter(), new PlanSubChapter(), requestActivity, activityToUpdate.Id));
            }

            context.UpdateRange(activitiesToUpdate);
            await context.SaveChangesAsync();
        }

        private async Task UpdateCustomSubChapter(PlanSubChapter planSubChapter, int userId, List<SelectedPlanActivity> customPlanActivities, PlanChapter chapter) {

            var subChapterToUpdate = await context.CustomSubchapter.Where(sChap => sChap.Id == planSubChapter.Id)
                    .Include(sChap => sChap.CustomActivities).FirstOrDefaultAsync();

            subChapterToUpdate.Title = planSubChapter.Title;
            subChapterToUpdate.ModifiedBy = userId;
            subChapterToUpdate.UpdateDate = DateTime.Now;

            foreach (var activityToUpdate in subChapterToUpdate.CustomActivities) {
                var requestActivity = planSubChapter.Activities.Where(act => act.Id == activityToUpdate.Id).FirstOrDefault();
                if (requestActivity != null) {
                    //activityToUpdate.Title = requestActivity.Title;
                    activityToUpdate.Description = requestActivity.Description;
                    activityToUpdate.ModifiedBy = userId;
                    activityToUpdate.UpdateDate = DateTime.Now;

                    customPlanActivities.Add(CreateCustomPlanActivity(chapter, planSubChapter, requestActivity, activityToUpdate.Id));
                }
            }



            context.UpdateRange(subChapterToUpdate);
            await context.SaveChangesAsync();

            //Clean attached Entities to avoid Exception when Activities is updated (The instance of entity type ... cannot be updated)

            //context.Entry(subChapterToUpdate).State = EntityState.Detached;
            //foreach (var activity in subChapterToUpdate.CustomActivities) {
            //    context.Entry(activity).State = EntityState.Detached;
            //}

        }

        private async Task UpdateCustomSubChapters(List<PlanSubChapter> planSubChapters, int userId, List<SelectedPlanActivity> customPlanActivities) {
            var customSubChapterIds = planSubChapters.Select(x => x.Id).ToList();

            var subChaptersToUpdate = await context.CustomSubchapter.Where(sChap => customSubChapterIds.Contains(sChap.Id))
                    .Include(sChap => sChap.CustomActivities).ToListAsync();

            foreach (var subChapterToUpdate in subChaptersToUpdate) {
                var requestSubChapter = planSubChapters.Where(sChap => sChap.Id == subChapterToUpdate.Id).FirstOrDefault();

                subChapterToUpdate.Title = requestSubChapter.Title;
                subChapterToUpdate.ModifiedBy = userId;
                subChapterToUpdate.UpdateDate = DateTime.Now;

                foreach (var activityToUpdate in subChapterToUpdate.CustomActivities) {
                    var requestActivity = requestSubChapter.Activities.Where(act => act.Id == activityToUpdate.Id).FirstOrDefault();

                    //activityToUpdate.Title = requestActivity.Title;
                    activityToUpdate.Description = requestActivity.Description;
                    activityToUpdate.ModifiedBy = userId;
                    activityToUpdate.UpdateDate = DateTime.Now;

                    customPlanActivities.Add(CreateCustomPlanActivity(new PlanChapter(), requestSubChapter, requestActivity, activityToUpdate.Id));
                }
            }


            context.UpdateRange(subChaptersToUpdate);
            await context.SaveChangesAsync();

            //Clean attached Entities to avoid Exception when Activities is updated (The instance of entity type ... cannot be updated)
            foreach (var subchapter in subChaptersToUpdate) {
                context.Entry(subchapter).State = EntityState.Detached;
                foreach (var activity in subchapter.CustomActivities) {
                    context.Entry(activity).State = EntityState.Detached;
                }
            }
        }

        private async Task UpdateCustomChapter(PlanChapter planChapter, int userId, List<SelectedPlanActivity> customPlanActivities) {

            var chapterToUpdate = await context.CustomChapter.Where(chap => chap.Id == planChapter.Id)
            .Include(chap => chap.CustomSubchapters)
                .ThenInclude(sChap => sChap.CustomActivities).FirstOrDefaultAsync();

            chapterToUpdate.ModifiedBy = userId;
            chapterToUpdate.Title = planChapter.Title;
            chapterToUpdate.UpdateDate = DateTime.Now;

            foreach (var subChapterToUpdate in chapterToUpdate.CustomSubchapters) {
                var requestSubChapter = planChapter.SubChapter.Where(sChap => sChap.Id == subChapterToUpdate.Id).FirstOrDefault();
                if (requestSubChapter != null) {

                    subChapterToUpdate.Title = requestSubChapter.Title;
                    subChapterToUpdate.ModifiedBy = userId;
                    subChapterToUpdate.UpdateDate = DateTime.Now;

                    foreach (var activityToUpdate in subChapterToUpdate.CustomActivities) {
                        var requestActivity = requestSubChapter.Activities.Where(act => act.Id == activityToUpdate.Id).FirstOrDefault();
                        if (requestActivity != null) {

                            //activityToUpdate.Title = requestActivity.Title;
                            activityToUpdate.Description = requestActivity.Description;
                            activityToUpdate.ModifiedBy = userId;
                            activityToUpdate.UpdateDate = DateTime.Now;

                            customPlanActivities.Add(CreateCustomPlanActivity(planChapter, requestSubChapter, requestActivity, activityToUpdate.Id));
                        }
                    }
                }
            }

            //foreach (var chapterToUpdate in chapterToUpdate) {
            //    var requestChapter = planChapter.Where(chap => chap.Id == chapterToUpdate.Id).FirstOrDefault();

            //    chapterToUpdate.Title = requestChapter.Title;
            //    chapterToUpdate.ModifiedBy = userId;
            //    chapterToUpdate.UpdateDate = DateTime.Now;

            //    foreach (var subChapterToUpdate in chapterToUpdate.CustomSubchapters) {
            //        var requestSubChapter = requestChapter.SubChapter.Where(sChap => sChap.Id == subChapterToUpdate.Id).FirstOrDefault();

            //        subChapterToUpdate.Title = requestSubChapter.Title;
            //        subChapterToUpdate.ModifiedBy = userId;
            //        subChapterToUpdate.UpdateDate = DateTime.Now;

            //        foreach (var activityToUpdate in subChapterToUpdate.CustomActivities) {
            //            var requestActivity = requestSubChapter.Activities.Where(act => act.Id == activityToUpdate.Id).FirstOrDefault();

            //            activityToUpdate.Title = requestActivity.Title;
            //            activityToUpdate.ModifiedBy = userId;
            //            activityToUpdate.UpdateDate = DateTime.Now;

            //            customPlanActivities.Add(CreateCustomPlanActivity(requestChapter, requestSubChapter, requestActivity, activityToUpdate.Id));
            //        }
            //    }
            //}

            context.Update(chapterToUpdate);
            await context.SaveChangesAsync();

            //Clean attached Entities to avoid Exception when Subchapter is updated (The instance of entity type ... cannot be updated)

            //context.Entry(chapterToUpdate).State = EntityState.Detached;
            //foreach (var subChapter in chapterToUpdate.CustomSubchapters) {
            //    context.Entry(subChapter).State = EntityState.Detached;
            //}

        }

        private async Task UpdateCustomChapters(List<PlanChapter> planChapters, int userId, List<SelectedPlanActivity> customPlanActivities) {
            var customChapterIds = planChapters.Select(x => x.Id).ToList();

            var chaptersToUpdate = await context.CustomChapter.Where(chap => customChapterIds.Contains(chap.Id))
                .Include(chap => chap.CustomSubchapters)
                    .ThenInclude(sChap => sChap.CustomActivities).ToListAsync();

            foreach (var chapterToUpdate in chaptersToUpdate) {
                var requestChapter = planChapters.Where(chap => chap.Id == chapterToUpdate.Id).FirstOrDefault();

                chapterToUpdate.Title = requestChapter.Title;
                chapterToUpdate.ModifiedBy = userId;
                chapterToUpdate.UpdateDate = DateTime.Now;

                foreach (var subChapterToUpdate in chapterToUpdate.CustomSubchapters) {
                    var requestSubChapter = requestChapter.SubChapter.Where(sChap => sChap.Id == subChapterToUpdate.Id).FirstOrDefault();

                    subChapterToUpdate.Title = requestSubChapter.Title;
                    subChapterToUpdate.ModifiedBy = userId;
                    subChapterToUpdate.UpdateDate = DateTime.Now;

                    foreach (var activityToUpdate in subChapterToUpdate.CustomActivities) {
                        var requestActivity = requestSubChapter.Activities.Where(act => act.Id == activityToUpdate.Id).FirstOrDefault();

                        //activityToUpdate.Title = requestActivity.Title;
                        activityToUpdate.Description = requestActivity.Description;
                        activityToUpdate.ModifiedBy = userId;
                        activityToUpdate.UpdateDate = DateTime.Now;

                        customPlanActivities.Add(CreateCustomPlanActivity(requestChapter, requestSubChapter, requestActivity, activityToUpdate.Id));
                    }
                }
            }

            context.UpdateRange(chaptersToUpdate);
            await context.SaveChangesAsync();

            ////Check if necessary with last changes
            ////Clean attached Entities to avoid Exception when Subchapter is updated (The instance of entity type ... cannot be updated)
            //foreach (var chapter in chaptersToUpdate) {
            //    context.Entry(chapter).State = EntityState.Detached;
            //    foreach (var subChapter in chapter.CustomSubchapters) {
            //        context.Entry(subChapter).State = EntityState.Detached;
            //    }
            //}
        }

        private SelectedPlanActivity CreateCustomPlanActivity(PlanChapter chapter, PlanSubChapter subchapter, PlanActivity activity, int activityId) => new SelectedPlanActivity {
            ActivityPosition = activity.Position,
            WordDescription = activity.WordDescription,
            ChapterDescription = chapter.WordDescription,
            ChapterPosition = chapter.Position,
            IsCustomActivity = true,
            SubChapterDescription = subchapter.WordDescription,
            SubChapterPosition = subchapter.Position,
            CustomActivityId = activityId
        };
    }
}
