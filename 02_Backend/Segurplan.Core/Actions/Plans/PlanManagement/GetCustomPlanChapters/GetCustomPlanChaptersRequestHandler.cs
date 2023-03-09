using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.GetCustomPlanChapters {
    public class GetCustomPlanChaptersRequestHandler : IRequestHandler<GetCustomPlanChaptersRequest, IRequestResponse<GetCustomPlanChaptersResponse>> {

        private readonly SegurplanContext context;

        public GetCustomPlanChaptersRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<GetCustomPlanChaptersResponse>> Handle(GetCustomPlanChaptersRequest request, CancellationToken cancellationToken) {

            SelectedPlanActivity planActivity = new SelectedPlanActivity();

            var activitiesList = new List<CustomActivity>();
            var subchaptersList = new List<CustomSubchapter>();
            var chaptersList = new List<CustomChapter>();
            
            var planActivitiesList = new List<CustomPlanActivity>();
            var planSubchaptersList = new List<CustomPlanSubchapter>();
            var planChaptersList = new List<CustomPlanChapter>();

            var customActivityIds = request.SelectedActivities.Select(x => x.CustomActivityId).ToList();
            var customSubChapterIds = new List<int>();
            var customChapterIds = new List<int>();

            var customActivities = await context.CustomActivity
                   .Where(x => customActivityIds.Contains(x.Id)).ToListAsync();

            foreach (var customActivity in customActivities) {
                if (customActivity.CustomSubchapterId != default) {
                    customSubChapterIds.Add(customActivity.CustomSubchapterId??0);
                }
            }

            if (customSubChapterIds.Any()) {

                customSubChapterIds = customSubChapterIds.Distinct().ToList();

                subchaptersList = await context.CustomSubchapter
                    .Where(x => customSubChapterIds.Contains(x.Id)).ToListAsync();

                foreach (var customSubChapter in subchaptersList) {
                    if (customSubChapter.CustomChapterId != default) {
                        customChapterIds.Add(customSubChapter.CustomChapterId??0);
                    }
                }
            }

            if (customChapterIds.Any()) {

                customChapterIds = customChapterIds.Distinct().ToList();

                chaptersList = await context.CustomChapter
                    .Where(x => customChapterIds.Contains(x.Id)).ToListAsync();
            }

            foreach (var customActivity in customActivities) {

                planActivity = request.SelectedActivities.Where(x => x.CustomActivityId == customActivity.Id).FirstOrDefault();
                //planActivity = request.SelectedActivities.Where(x => x.CustomActivityId == item.Id).FirstOrDefault();

                var customPlanActivity = new CustomPlanActivity {
                    ChapterPosition=planActivity.ChapterPosition ,
                    SubChapterPosition=planActivity.SubChapterPosition,
                    Description=customActivity.Description,
                    Id=customActivity.Id,
                    //Number=0,
                    Position=planActivity.ActivityPosition,
                    Title=customActivity.Title,
                    WordDescription=planActivity.WordDescription,
                    CustomSubChapterId=customActivity.CustomSubchapterId??0,
                    SubChapterId=customActivity.SubChapterId
                };

                planActivitiesList.Add(customPlanActivity);

                if (customActivity.CustomSubchapterId != default && !planSubchaptersList.Any(x=>x.Id==customActivity.CustomSubchapterId)) {

                    var planSubchapter = subchaptersList.Where(x => x.Id == customActivity.CustomSubchapterId).FirstOrDefault();

                    var customPlanSubchapter = new CustomPlanSubchapter {
                        //Activities=,
                        ChapterPosition=planActivity.ChapterPosition,
                        CustomChapterId=planSubchapter.CustomChapterId??0,
                        Description=planSubchapter.Description,
                        Id=planSubchapter.Id,
                        //Number=,
                        Position=planActivity.SubChapterPosition,
                        Title=planSubchapter.Title,
                        WordDescription= planActivity.SubChapterDescription,
                        ChapterId=planSubchapter.ChapterId
                    };

                    planSubchaptersList.Add(customPlanSubchapter);

                    if (planSubchapter.CustomChapterId != default && !planChaptersList.Any(x => x.Id == planSubchapter.CustomChapterId)) {

                        var planChapter = chaptersList.Where(x => x.Id == planSubchapter.CustomChapterId).FirstOrDefault();

                        var customPlanChapter = new CustomPlanChapter {
                            Id = planChapter.Id,
                            //Number=,
                            Position = planActivity.ChapterPosition,
                            //Subchapters=,
                            Title = planChapter.Title,
                            WordDescription = planActivity.ChapterDescription ?? planChapter.Description
                        };

                        planChaptersList.Add(customPlanChapter);
                    }
                }
            }

            foreach (var planSubchapter in planSubchaptersList) {
                planSubchapter.Activities = planActivitiesList.Where(x => x.CustomSubChapterId == planSubchapter.Id).ToList();

                planActivitiesList.RemoveAll(x => x.CustomSubChapterId == planSubchapter.Id);
            }

            foreach (var planChapter in planChaptersList) {
                planChapter.Subchapters = planSubchaptersList.Where(x => x.CustomChapterId == planChapter.Id).ToList();

                planSubchaptersList.RemoveAll(x => x.CustomChapterId == planChapter.Id);
            }

            //var customPlanChapters = await context.CustomChapter.Where(x => x.CustomSubchapters.Any(cs => cs.CustomActivities.Any(ca => customActivityIds.Contains(ca.Id)))).Select(cCh => new CustomPlanChapter {
            //    Id = cCh.Id,
            //    Title = cCh.Title,
            //    Subchapters = cCh.CustomSubchapters.Select(cSc => new CustomPlanSubchapter {
            //        Id = cSc.Id,
            //        Description = cSc.Description,
            //        Position = request.SelectedActivities.Where(x => cSc.CustomActivities.Any(act => customActivityIds.Contains(x.Id))).Select(z => z.SubChapterPosition).FirstOrDefault(),
            //        Title = cSc.Title,
            //        Activities = cSc.CustomActivities.Select(cAct => new CustomPlanActivity {
            //            Id = cAct.Id,
            //            Title = cAct.Title,
            //            Position = request.SelectedActivities.Where(x => x.CustomActivityId == cAct.Id).FirstOrDefault().ActivityPosition,
            //        }).ToList()
            //    }).ToList()
            //}).ToListAsync();

            //var customActivities = customPlanChapters.SelectMany(x => x.Subchapters.SelectMany(y => y.Activities)).ToList();

            

            //foreach (var item in customActivities) {

            //    planActivity = request.SelectedActivities.Where(x => x.CustomActivityId == item.Id).FirstOrDefault();

            //    item.Position = planActivity.ActivityPosition;
            //    item.WordDescription = planActivity.WordDescription;

            //    var customChapters = customPlanChapters.Where(ch => ch.Subchapters.Any(sch => sch.Activities.Any(ac => ac.Id == item.Id))).FirstOrDefault();

            //    if (customChapters.Position == 0) {
            //        customChapters.Position = planActivity.ChapterPosition;
            //        customChapters.WordDescription = planActivity.ChapterDescription;

            //        var customSubChapters = customChapters.Subchapters.Where(x => x.Activities.Any(sch => sch.Id == item.Id)).FirstOrDefault();
            //        if (customSubChapters.Position == 0) {
            //            customSubChapters.Position = planActivity.SubChapterPosition;
            //            customSubChapters.WordDescription = planActivity.SubChapterDescription;
            //        }
            //    }
            //}

            OrderCustomPlanChapters(planChaptersList);
            OrderCustomPlanSubChapters(planSubchaptersList);
            OrderCustomPlanActivities(planActivitiesList);

            return planChaptersList.Count() > 0 || planSubchaptersList.Count() > 0|| planActivitiesList.Count() > 0 
               ? RequestResponse.Ok(new GetCustomPlanChaptersResponse(planChaptersList,planSubchaptersList,planActivitiesList))
               : RequestResponse.NotFound<GetCustomPlanChaptersResponse>();

            //return customPlanChapters.Count() > 0 ? RequestResponse.Ok(new GetCustomPlanChaptersResponse(customPlanChapters))
            //    : RequestResponse.NotFound<GetCustomPlanChaptersResponse>();
        }

        private void OrderCustomPlanChapters(List<CustomPlanChapter> customPlanChapters) {

            if (customPlanChapters.Any()) {
                customPlanChapters.OrderBy(pCh => pCh.Position)
                .ThenBy(pCh => pCh.Subchapters.OrderBy(pSch => pSch.Position)
                    .ThenBy(pSch => pSch.Activities.OrderBy(pAct => pAct.Position)));
            }           
        }

        private void OrderCustomPlanSubChapters(List<CustomPlanSubchapter> customPlanSubChapters) {

            if (customPlanSubChapters.Any()) {
                customPlanSubChapters.OrderBy(pCh => pCh.Position)
                    .ThenBy(pSch => pSch.Activities.OrderBy(pAct => pAct.Position));
            }
        }

        private void OrderCustomPlanActivities(List<CustomPlanActivity> customPlanActivities) {

            if (customPlanActivities.Any()) {
                customPlanActivities.OrderBy(pCh => pCh.Position);
            }
        }
    }
}
