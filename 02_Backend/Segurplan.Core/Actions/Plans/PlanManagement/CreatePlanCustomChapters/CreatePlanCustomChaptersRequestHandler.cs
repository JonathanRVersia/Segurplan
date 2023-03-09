using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.CreatePlanCustomChapters {
    public class CreatePlanCustomChaptersRequestHandler : IRequestHandler<CreatePlanCustomChaptersRequest, IRequestResponse<CreatePlanCustomChaptersResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;

        public CreatePlanCustomChaptersRequestHandler(SegurplanContext context, IMapper mapper, IHttpContextAccessor contextAccessor) {
            this.context = context;
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IRequestResponse<CreatePlanCustomChaptersResponse>> Handle(CreatePlanCustomChaptersRequest request, CancellationToken cancellationToken) {

            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<SelectedPlanActivity> customPlanActivities = new List<SelectedPlanActivity>();
            List<PlanChapter> newCustomChapters = new List<PlanChapter>();
            List<PlanSubChapter> newCustomSubChapters = new List<PlanSubChapter>();
            List<PlanActivity> newCustomActivities = new List<PlanActivity>();

            //if (request.Chapters.Any(chap => chap.IsCustomChapter && chap.Id == 0))
            //    await CreateCustomChapters(request.Chapters.Where(chap => chap.IsCustomChapter).ToList(), customPlanActivities, userId);
            //if (request.Chapters.SelectMany(chap => chap.SubChapter).Any(sChap => sChap.IsCustomSubChapter))

            //Si hay alguna actividadCustom es obligatorio que se haya creado o Capitulo o SubCapitulo
            if (request.Chapters.SelectMany(chap => chap.SubChapter).Any(sChap => sChap.Activities.Any(act => act.IsCustomActivity && act.Id == 0))) {
                foreach (var chapter in request.Chapters) {

                    if (chapter.IsCustomChapter && chapter.Id == 0) {
                        await CreateCustomChapter(chapter, customPlanActivities, userId);
                    } else {
                        foreach (var subChapter in chapter.SubChapter) {

                            if (subChapter.IsCustomSubChapter && subChapter.Id == 0) {
                                await CreateCustomSubChapter(subChapter, customPlanActivities, userId, chapter);
                            } else {
                                foreach (var activity in subChapter.Activities) {

                                    if (activity.IsCustomActivity && activity.Id == 0) {
                                        await CreateCustomActivity(activity, customPlanActivities, userId, chapter, subChapter);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            //var newCustomSubChapters = chapters.SelectMany(x => x.SubChapter).Where(sub => sub.IsCustomSubChapter && sub.Id == 0).ToList();
            //var newCustomActivities = chapters.SelectMany(x => x.SubChapter.SelectMany(sub => sub.Activities).Where(act => act.IsCustomActivity && act.Id == 0)).ToList();

            //if (request.NewCustomSubChapters.Any())
            //    await CreateCustomSubChapters(request.NewCustomSubChapters, customPlanActivities, userId);

            //if (request.NewCustomActivities.Any())
            //    await CreateCustomActivities(request.NewCustomActivities, customPlanActivities, userId);

            return customPlanActivities.Count() > 0 ? RequestResponse.Ok(new CreatePlanCustomChaptersResponse(customPlanActivities))
                    : RequestResponse.NotOk<CreatePlanCustomChaptersResponse>();
        }

        private async Task CreateCustomActivity(PlanActivity newCustomActivity, List<SelectedPlanActivity> customPlanActivities, int userId, PlanChapter chapter, PlanSubChapter subChapter) {

            var customActivity = mapper.Map<CustomActivity>(newCustomActivity);

            customActivity.ModifiedBy = userId;
            customActivity.CreatedBy = userId;
            if (customActivity.SubChapterId != 0) {
                if (subChapter.IsCustomSubChapter) {
                    customActivity.CustomSubchapterId = subChapter.Id;
                    customActivity.SubChapterId = null;
                } else {
                    customActivity.SubChapterId = subChapter.Id;
                }

            } else {
                customActivity.SubChapterId = null;
            }

            context.Add(customActivity);
            await context.SaveChangesAsync();

            customPlanActivities.Add(new SelectedPlanActivity {
                ActivityPosition = newCustomActivity.Position,
                //ChapterDescription = newCustomChapters[i].WordDescription,
                ChapterPosition = chapter.Position,
                IsCustomActivity = true,
                //SubChapterDescription = newCustomActivities[i].WordDescriptionf,
                SubChapterPosition = subChapter.Position,
                WordDescription = newCustomActivity.WordDescription,
                CustomActivityId = customActivity.Id
            });

        }

        //private async Task CreateCustomActivities(List<PlanActivity> newCustomActivities, List<SelectedPlanActivity> customPlanActivities, int userId) {
        //    for (int i = 0; i < newCustomActivities.Count(); i++) {
        //        var customActivities = mapper.Map<CustomActivity>(newCustomActivities[i]);

        //        customActivities.ModifiedBy = userId;
        //        customActivities.CreatedBy = userId;

        //        context.Add(customActivities);
        //        await context.SaveChangesAsync();

        //        customPlanActivities.Add(new SelectedPlanActivity {
        //            ActivityPosition = newCustomActivities[i].Position,
        //            //ChapterDescription = newCustomChapters[i].WordDescription,//Traerlo en el request
        //            //ChapterPosition = newCustomChapters[i].Position,//Traerlo en el request
        //            IsCustomActivity = true,
        //            //SubChapterDescription = newCustomActivities[i].WordDescriptionf,//Traerlo en el request
        //            //SubChapterPosition = newCustomActivities[i].Positionf,//Traerlo en el request
        //            WordDescription = newCustomActivities[i].WordDescription,
        //            CustomActivityId = customActivities.Id
        //        });
        //    }
        //}

        private async Task CreateCustomSubChapter(PlanSubChapter newCustomSubChapter, List<SelectedPlanActivity> customPlanActivities, int userId, PlanChapter chapter) {

            var customSubChapterToAdd = mapper.Map<CustomSubchapter>(newCustomSubChapter);

            customSubChapterToAdd.ModifiedBy = userId;
            customSubChapterToAdd.CreatedBy = userId;
            if (chapter.Id != 0) {                
                if (chapter.IsCustomChapter) {
                    customSubChapterToAdd.CustomChapterId = chapter.Id;
                    customSubChapterToAdd.ChapterId = null;
                } 
                else 
                {
                    customSubChapterToAdd.ChapterId = chapter.Id;
                }
            }
            else {
                customSubChapterToAdd.ChapterId = null;
            }


            customSubChapterToAdd.CustomActivities.Select(act => {
                act.CreatedBy = userId;
                act.ModifiedBy = userId;
                act.SubChapterId = null;
                return act;
            }).ToList();

            context.Add(customSubChapterToAdd);
            await context.SaveChangesAsync();

            for (int k = 0; k < customSubChapterToAdd.CustomActivities.Count(); k++) {
                customPlanActivities.Add(new SelectedPlanActivity {
                    ActivityPosition = newCustomSubChapter.Activities[k].Position,
                    //ChapterDescription = newCustomChapters[i].WordDescription,//Traerlo en el request
                    ChapterPosition = chapter.Position,
                    IsCustomActivity = true,
                    SubChapterDescription = newCustomSubChapter.WordDescription,
                    SubChapterPosition = newCustomSubChapter.Position,
                    WordDescription = newCustomSubChapter.Activities[k].WordDescription,
                    CustomActivityId = customSubChapterToAdd.CustomActivities[k].Id
                });
            }


        }

        //private async Task CreateCustomSubChapters(List<PlanSubChapter> newCustomSubChapters, List<SelectedPlanActivity> customPlanActivities, int userId) {
        //    for (int i = 0; i < newCustomSubChapters.Count(); i++) {
        //        var customSubChapterToAdd = mapper.Map<CustomSubchapter>(newCustomSubChapters[i]);

        //        customSubChapterToAdd.ModifiedBy = userId;
        //        customSubChapterToAdd.CreatedBy = userId;

        //        customSubChapterToAdd.CustomActivities.Select(act => {
        //            act.CreatedBy = userId;
        //            act.ModifiedBy = userId;
        //            return act;
        //        }).ToList();

        //        context.Add(customSubChapterToAdd);
        //        await context.SaveChangesAsync();

        //        for (int k = 0; k < customSubChapterToAdd.CustomActivities.Count(); k++) {
        //            customPlanActivities.Add(new SelectedPlanActivity {
        //                ActivityPosition = newCustomSubChapters[i].Activities[k].Position,
        //                //ChapterDescription = newCustomChapters[i].WordDescription,//Traerlo en el request
        //                //ChapterPosition = newCustomChapters[i].Position,//Traerlo en el request
        //                IsCustomActivity = true,
        //                SubChapterDescription = newCustomSubChapters[i].WordDescription,
        //                SubChapterPosition = newCustomSubChapters[i].Position,
        //                WordDescription = newCustomSubChapters[i].Activities[k].WordDescription,
        //                CustomActivityId = customSubChapterToAdd.CustomActivities[k].Id
        //            });
        //        }

        //    }
        //}

        private async Task CreateCustomChapter(PlanChapter newCustomChapter, List<SelectedPlanActivity> customPlanActivities, int userId) {

            var customChapterToAdd = mapper.Map<CustomChapter>(newCustomChapter);

            customChapterToAdd.ModifiedBy = userId;
            customChapterToAdd.CreatedBy = userId;

            customChapterToAdd.CustomSubchapters.Select(schap => {
                schap.CreatedBy = userId;
                schap.ModifiedBy = userId;
                schap.ChapterId = null;
                schap.CustomActivities.Select(act => {
                    act.CreatedBy = userId;
                    act.ModifiedBy = userId;
                    act.SubChapterId = null;
                    return act;
                }).ToList();
                return schap;
            }).ToList();

            context.Add(customChapterToAdd);
            await context.SaveChangesAsync();

            for (int j = 0; j < customChapterToAdd.CustomSubchapters.Count; j++) {

                for (int k = 0; k < customChapterToAdd.CustomSubchapters[j].CustomActivities.Count(); k++) {
                    customPlanActivities.Add(new SelectedPlanActivity {
                        ActivityPosition = newCustomChapter.SubChapter[j].Activities[k].Position,
                        ChapterDescription = newCustomChapter.WordDescription,
                        ChapterPosition = newCustomChapter.Position,
                        IsCustomActivity = true,
                        SubChapterDescription = newCustomChapter.SubChapter[j].WordDescription,
                        SubChapterPosition = newCustomChapter.SubChapter[j].Position,
                        WordDescription = newCustomChapter.SubChapter[j].Activities[k].WordDescription,
                        CustomActivityId = customChapterToAdd.CustomSubchapters[j].CustomActivities[k].Id
                    });
                }
            }

        }

        //private async Task CreateCustomChapters(List<PlanChapter> newCustomChapters, List<SelectedPlanActivity> customPlanActivities, int userId) {
        //    for (int i = 0; i < newCustomChapters.Count(); i++) {
        //        var customChapterToAdd = mapper.Map<CustomChapter>(newCustomChapters[i]);

        //        customChapterToAdd.ModifiedBy = userId;
        //        customChapterToAdd.CreatedBy = userId;

        //        customChapterToAdd.CustomSubchapters.Select(schap => {
        //            schap.CreatedBy = userId;
        //            schap.ModifiedBy = userId;
        //            schap.CustomActivities.Select(act => {
        //                act.CreatedBy = userId;
        //                act.ModifiedBy = userId;
        //                return act;
        //            }).ToList();
        //            return schap;
        //        }).ToList();

        //        context.Add(customChapterToAdd);
        //        await context.SaveChangesAsync();

        //        for (int j = 0; j < customChapterToAdd.CustomSubchapters.Count; j++) {

        //            for (int k = 0; k < customChapterToAdd.CustomSubchapters[j].CustomActivities.Count(); k++) {
        //                customPlanActivities.Add(new SelectedPlanActivity {
        //                    ActivityPosition = newCustomChapters[i].SubChapter[j].Activities[k].Position,
        //                    ChapterDescription = newCustomChapters[i].WordDescription,
        //                    ChapterPosition = newCustomChapters[i].Position,
        //                    IsCustomActivity = true,
        //                    SubChapterDescription = newCustomChapters[i].SubChapter[j].WordDescription,
        //                    SubChapterPosition = newCustomChapters[i].SubChapter[j].Position,
        //                    WordDescription = newCustomChapters[i].SubChapter[j].Activities[k].WordDescription,
        //                    CustomActivityId = customChapterToAdd.CustomSubchapters[j].CustomActivities[k].Id
        //                });
        //            }
        //        }
        //    }
        //}
    }
}
