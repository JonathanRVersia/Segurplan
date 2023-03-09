using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.Core.Domain.CacheServices;
using Segurplan.Core.Extensions.Comparers;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.MemoryCache;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View {
    public class ViewPlanActivitiesRequestHandler : IRequestHandler<ViewPlanActivitiesRequest, IRequestResponse<ViewPlanActivitiesResponse>> {

        private readonly SegurplanContext dbContext;
        private readonly MemoryCacheService cacheService;
        private readonly ActivitiesCacheService activitiesCacheServices;
        private readonly IMapper mapper;

        public ViewPlanActivitiesRequestHandler(SegurplanContext dbContext, MemoryCacheService cacheService, ActivitiesCacheService activitiesCacheServices, IMapper mapper) {
            this.dbContext = dbContext;
            this.cacheService = cacheService;
            this.activitiesCacheServices = activitiesCacheServices;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<ViewPlanActivitiesResponse>> Handle(ViewPlanActivitiesRequest request, CancellationToken cancellationToken) {
            try {

                var lists = await GetActivities(request.PlanId, request.GetAll);

                return RequestResponse.Ok(new ViewPlanActivitiesResponse {
                    ActivityLists = lists
                });

            } catch (Exception exc) {

                throw exc;
            }

        }

        private async Task<SafetyPlanActivities> GetActivities(int planId, bool getAll) {

            try {

                return new SafetyPlanActivities {

                    AvailableActivities = getAll ? await GetAvailableActivities() : new List<PlanChapter>(0),
                    PlanActivities = planId > 0 ? await GetPlanActivities(planId) : new List<SelectedPlanActivity>(0)
                };
            } catch (Exception exc) {

                throw;
            }
        }

        private async Task<List<PlanChapter>> GetAvailableActivities() {

            //List<PlanChapter> planChapters = (List<PlanChapter>)await activitiesCacheServices.Get();
            List<PlanChapter> planChapters = await dbContext.ChapterVersion.Where(x => x.ApprovementDate < DateTime.Now && x.EndDate == null || x.EndDate > DateTime.Now)
               .ProjectTo<PlanChapter>(mapper.ConfigurationProvider)
              .ToListAsync();

            if (planChapters.Any()) {
                planChapters = planChapters.OrderBy(x => x.Number).ToList();


                // Ensuring all available activities are returns as NOT SELECTED
                planChapters.Select(chap => {
                    chap.IsSelected = false;
                    chap.SubChapter.Select(sub => {
                        sub.IsSelected = false;
                        sub.Activities.Select(act => {
                            act.IsSelected = false;
                            return act;
                        }).ToList();
                        return sub;
                    }).ToList();
                    return chap;
                }).ToList();


                //var selectedChapts = planChapters.Where(c => c.SubChapter.Any(s => s.Activities.Any(a => a.IsSelected == true)));

                //foreach (var sub in from chap in selectedChapts
                //                    from sub in chap.SubChapter
                //                    select sub) {
                //    sub.Activities.Select(act => { act.IsSelected = false; return act; }).ToList();
                //}
            }

            return planChapters;
        }

        private async Task<List<SelectedPlanActivity>> GetPlanActivities(int planId) {
            return await (from pAct in dbContext.PlanActivityVersion
                          where pAct.IdPlan == planId
                          select new SelectedPlanActivity {
                              Id = pAct.IdActivityVersionNavigation != null ? pAct.IdActivityVersionNavigation.IdActivity : default,
                              ActivityPosition = pAct.Position,
                              WordDescription = pAct.WordDescription,
                              ChapterPosition = pAct.ChapterPosition,
                              SubChapterPosition = pAct.SubChapterPosition,
                              ChapterDescription = pAct.ChapterDescription,
                              SubChaptId = pAct.SubChaptId,
                              SubChapterDescription = pAct.SubChapterDescription,
                              AvailableActivitiId = pAct.AvailableActivitiId,
                              CustomActivityId = pAct.CustomActivityId ?? default,
                              IsCustomActivity = pAct.CustomActivityId > 0,
                              IdActivityVersion = pAct.IdActivityVersion ?? default,
                          }).ToListAsync();
        }

        private List<PlanChapter> FormatList(List<PlanChapter> dbChapters) {

            List<PlanChapter> convertedList = new List<PlanChapter>();

            // Getting the different chapters
            var chapList = dbChapters.Distinct(new PlanChapterComparer()).OrderBy(c => c.Number);

            foreach (var chap in chapList) {

                PlanChapter convertedChap = new PlanChapter {
                    Id = chap.Id,
                    IsSelected = false,
                    Position = chap.Number,
                    Number = chap.Number,
                    Title = chap.Title.TrimStart(),
                    DefaultSelectedChapter = chap.DefaultSelectedChapter,
                    SubChapter = new List<PlanSubChapter>()
                };

                // Getting all different subchapters of the given chap
                var chapSubs = dbChapters.Where(c => c.Id == chap.Id)
                    .Select(ch => ch.SubChapter.FirstOrDefault())
                    .Distinct(new PlanSubChapterComparer())
                    .OrderBy(s => s.Number)
                    .ToList();

                foreach (var subChap in chapSubs) {

                    PlanSubChapter convertedSubChap = new PlanSubChapter {
                        Id = subChap.Id,
                        IsSelected = false,
                        Position = subChap.Number,
                        Number = subChap.Number,
                        Title = subChap.Title.TrimStart(),
                        Description = subChap.Description,
                        Activities = new List<PlanActivity>()
                    };

                    // Getting all activities of the given subchapter
                    var subActs = dbChapters
                        .Where(c => c.Id == chap.Id)
                        .Select(sub => sub.SubChapter.Where(s => s.Id == subChap.Id)
                            .Select(a => a.Activities.FirstOrDefault())
                            .Distinct(new PlanActivityComparer())
                            .Select(a => new PlanActivity {
                                Id = a.Id,
                                IsSelected = false,
                                Position = a.Number,
                                Number = a.Number,
                                Description = a.Description.TrimStart(),
                                WordDescription = a.WordDescription
                            }).FirstOrDefault())
                        .Where(a => a != null).ToList();

                    convertedSubChap.Activities.AddRange(subActs);

                    convertedChap.SubChapter.Add(convertedSubChap);
                }

                convertedList.Add(convertedChap);
            }

            return convertedList;
        }
    }
}
