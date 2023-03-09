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
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.DuplicatePlanCustomChapters {
    public class DuplicatePlanCustomChaptersRequestHandler : IRequestHandler<DuplicatePlanCustomChaptersRequest, IRequestResponse<DuplicatePlanCustomChaptersResponse>> {

        private readonly SegurplanContext context;
        private readonly IHttpContextAccessor contextAccessor;

        private List<CustomActivity> CustomActivitiesToCopy = new List<CustomActivity>();
        private List<CustomSubchapter> CustomSubchaptersToCopy = new List<CustomSubchapter>();
        private List<CustomChapter> CustomChaptersToCopy = new List<CustomChapter>();

        private List<CustomActivity> NewCustomActivities = new List<CustomActivity>();
        private List<CustomSubchapter> NewCustomSubchapters = new List<CustomSubchapter>();
        private List<CustomChapter> NewCustomChapters = new List<CustomChapter>();

        //Used to update the id on PlanActivityVersion
        private Dictionary<CustomActivity, CustomActivity> NewAndOldActivities = new Dictionary<CustomActivity, CustomActivity>();

        private bool IsCreate = false;

        public DuplicatePlanCustomChaptersRequestHandler(SegurplanContext context, IHttpContextAccessor contextAccessor) {
            this.context = context;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IRequestResponse<DuplicatePlanCustomChaptersResponse>> Handle(DuplicatePlanCustomChaptersRequest request, CancellationToken cancellationToken) {

            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int changes = 0;

            //Ensures only copying customActivities
            request.PlanActivityVersions.RemoveAll(x => x.CustomActivityId == null);

            await GetOriginalData(request);

            if (CustomChaptersToCopy.Any())
                CreateCustomChapters(userId);

            if (CustomSubchaptersToCopy.Any())
                CreateCustomSubChapters(userId);

            if (CustomActivitiesToCopy.Any())
                CreateCustomActivities(userId);

            if (NewCustomChapters.Any())
                await context.AddRangeAsync(NewCustomChapters);

            if (NewCustomSubchapters.Any())
                await context.AddRangeAsync(NewCustomSubchapters);

            if (NewCustomActivities.Any())
                await context.AddRangeAsync(NewCustomActivities);

            if (IsCreate)
                changes = await context.SaveChangesAsync();

            UpdatePlanActivityVersionToNewIds(request);

            return RequestResponse.Ok(new DuplicatePlanCustomChaptersResponse(request.PlanActivityVersions));
        }

        private void UpdatePlanActivityVersionToNewIds(DuplicatePlanCustomChaptersRequest request) {
            var newActivities = new List<CustomActivity>();

            if (NewCustomChapters.Any()) {
                newActivities.AddRange(NewCustomChapters.SelectMany(chap => chap.CustomSubchapters.SelectMany(sChap => sChap.CustomActivities).ToList()));
            }

            if (NewCustomSubchapters.Any()) {
                newActivities.AddRange(NewCustomSubchapters.SelectMany(sChap => sChap.CustomActivities).ToList());
            }

            if (NewCustomActivities.Any()) {
                newActivities.AddRange(NewCustomActivities);
            }

            foreach (var newActivity in newActivities) {

                var oldActivity = new CustomActivity();

                NewAndOldActivities.TryGetValue(newActivity, out oldActivity);

                var planActivityToUpdate = request.PlanActivityVersions.Where(x => x.CustomActivityId == oldActivity.Id).FirstOrDefault();

                planActivityToUpdate.CustomActivityId = newActivity.Id;
            }
        }

        private void CreateCustomChapters(int userId) {

            foreach (var chapterToCopy in CustomChaptersToCopy) {

                var newCustomChapter = new CustomChapter {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    ModifiedBy = userId,
                    CreatedBy = userId,
                    Description = chapterToCopy.Description,
                    Title = chapterToCopy.Title,
                    CustomSubchapters = new List<CustomSubchapter>()
                };

                var originalSubChapters = CustomSubchaptersToCopy.Where(x => x.CustomChapterId == chapterToCopy.Id).ToList();

                CustomSubchaptersToCopy.RemoveAll(x => x.CustomChapterId == chapterToCopy.Id);

                foreach (var subchapterToCopy in originalSubChapters) {
                    var newCustomSubChapter = new CustomSubchapter {
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        CreatedBy = userId,
                        ModifiedBy = userId,
                        Description = subchapterToCopy.Description,
                        Title = subchapterToCopy.Title,
                        CustomActivities = new List<CustomActivity>()
                    };

                    var originalActivities = CustomActivitiesToCopy.Where(x => x.CustomSubchapterId == subchapterToCopy.Id).ToList();

                    CustomActivitiesToCopy.RemoveAll(x => x.CustomSubchapterId == subchapterToCopy.Id);

                    foreach (var activityToCopy in originalActivities) {

                        var newCustomActivity = new CustomActivity {
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            CreatedBy = userId,
                            ModifiedBy = userId,
                            Description = activityToCopy.Description,
                            Title = activityToCopy.Title
                        };

                        newCustomSubChapter.CustomActivities.Add(newCustomActivity);

                        NewAndOldActivities.Add(newCustomActivity,activityToCopy);
                    }

                    newCustomChapter.CustomSubchapters.Add(newCustomSubChapter);
                }

                NewCustomChapters.Add(newCustomChapter);
            }

            IsCreate = true;
        }

        private void CreateCustomSubChapters(int userId) {

            foreach (var subchapterToCopy in CustomSubchaptersToCopy) {
                var newCustomSubChapter = new CustomSubchapter {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    Description = subchapterToCopy.Description,
                    Title = subchapterToCopy.Title,
                    ChapterId = subchapterToCopy.ChapterId,
                    CustomActivities = new List<CustomActivity>()
                };

                var originalActivities = CustomActivitiesToCopy.Where(x => x.CustomSubchapterId == subchapterToCopy.Id).ToList();

                CustomActivitiesToCopy.RemoveAll(x => x.CustomSubchapterId == subchapterToCopy.Id);

                foreach (var activityToCopy in originalActivities) {

                    var newCustomActivity = new CustomActivity {
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        CreatedBy = userId,
                        ModifiedBy = userId,
                        Description = activityToCopy.Description,
                        Title = activityToCopy.Title
                    };

                    newCustomSubChapter.CustomActivities.Add(newCustomActivity);

                    NewAndOldActivities.Add(newCustomActivity, activityToCopy);
                }

                NewCustomSubchapters.Add(newCustomSubChapter);
            }

            IsCreate = true;
        }

        private void CreateCustomActivities(int userId) {

            foreach (var activityToCopy in CustomActivitiesToCopy) {

                var newCustomActivity = (new CustomActivity {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    Description = activityToCopy.Description,
                    Title = activityToCopy.Title,
                    SubChapterId = activityToCopy.SubChapterId
                });

                NewCustomActivities.Add(newCustomActivity);

                NewAndOldActivities.Add(newCustomActivity, activityToCopy);
            }

            IsCreate = true;
        }

        private async Task GetOriginalData(DuplicatePlanCustomChaptersRequest request) {

            if (request.PlanActivityVersions.Any()) {

                var customActivityIds = request.PlanActivityVersions.Select(x => x.CustomActivityId).ToList();

                CustomActivitiesToCopy = await context.CustomActivity.Where(cAct => customActivityIds.Contains(cAct.Id)).ToListAsync();

                if (CustomActivitiesToCopy.Any(x => x.CustomSubchapterId != null)) {

                    var customSubchapterIds = CustomActivitiesToCopy.Where(cAct => cAct.CustomSubchapterId != null).Select(cAct => cAct.CustomSubchapterId).ToList();

                    CustomSubchaptersToCopy = await context.CustomSubchapter.Where(sChap => customSubchapterIds.Contains(sChap.Id)).ToListAsync();

                    if (CustomSubchaptersToCopy.Any(x => x.CustomChapterId != null)) {

                        var customChapterIds = CustomSubchaptersToCopy.Where(sChap => sChap.CustomChapterId != null).Select(sChap => sChap.CustomChapterId).ToList();

                        CustomChaptersToCopy = await context.CustomChapter.Where(chap => customChapterIds.Contains(chap.Id)).ToListAsync();
                    }
                }

            }
        }
    }
}
