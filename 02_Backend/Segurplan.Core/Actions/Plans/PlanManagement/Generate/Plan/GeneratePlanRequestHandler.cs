using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DocumentFormat.OpenXml;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Segurplan.Core.Actions.AllDocuments.Documents;
using Segurplan.Core.Actions.AllDocuments.Helpers;
using Segurplan.Core.Actions.AllDocuments.Models;
using Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos;
using Segurplan.Core.Actions.Plans.PlanManagement.Files.Download;
using Segurplan.Core.Actions.Plans.PlanManagement.Files.View;
using Segurplan.Core.Actions.Plans.PlanManagement.Read;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.View;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Generate.Plan {

    public class GeneratePlanRequestHandler : IRequestHandler<GeneratePlanRequest, IRequestResponse<GeneratePlanResponse>> {

        private const string FORMATO_SIN_TABLA_COMPLETO = "Plan formato sin tablas completo";
        private const int ImageHeigthConstant = 66;
        private const int ImageWidthConstant = 152;
        private readonly SegurplanContext context;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private bool NeedActivityRelation = true;
        List<int> ChapterRelationList = new List<int>();
        List<int> SubchapterRelationList = new List<int>();
        List<int> ActivityRelationList = new List<int>();

        public GeneratePlanRequestHandler(SegurplanContext context, IMapper mapper, IMediator mediator) {
            this.context = context;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        public async Task<IRequestResponse<GeneratePlanResponse>> Handle(GeneratePlanRequest request, CancellationToken cancellationToken) {

            var dataResponse = await mediator.Send(new ViewPlanGeneralDataRequest(request.PlanId)).ConfigureAwait(true);

            var chapters = await GetChaptersAsync(request);

            DocumentContent planTemplateData = mapper.Map<DocumentContent>(dataResponse.Value.PlanInformation);
            planTemplateData.AnagramaHtml = await GetAnagramAsHtmlAsync(dataResponse, request);

            planTemplateData.ChaptersHtml = chapters;
            SetIndexAndCheckHtmlTags(planTemplateData);
            planTemplateData.RisksAndPreventiveMeasures = await GetPreventiveMeasures(chapters);
            CleanNulls(planTemplateData);
            planTemplateData.IsEvaluation = request.IsEvaluation;
            FillRisksPerActivityData(planTemplateData, request.TemplateName);
            FillRenderData(planTemplateData);
            CustomBusinessMappings.FillRisksPerActivityCharsData(planTemplateData);
            await FillBlueprintsAsync(dataResponse.Value.PlanInformation.AdditionalData.IdPlan, planTemplateData);
            await ApplyOrderByNumber(planTemplateData);

            planTemplateData.ArticleFamily = ConvertToBudgetDocument(dataResponse.Value.PlanInformation.IdBudget);
            planTemplateData.TotalBudgetPrice = planTemplateData.ArticleFamily.Sum(family => family.Price);
            IRequestResponse<CreateDocumentResponse> fileResponse = await mediator.Send(new CreateDocumentRequest(planTemplateData, request.TemplateName)).ConfigureAwait(false);


            if (fileResponse.Status != RequestStatus.Ok)
                return RequestResponse.NotFound<GeneratePlanResponse>();

            return RequestResponse.Ok(new GeneratePlanResponse(fileResponse.Value.Document));
        }

        private List<ApplicationArticleFamily> ConvertToBudgetDocument(int idBudget) {
            List<ApplicationArticleFamily> articlesByFamily = new List<ApplicationArticleFamily>();
            var budgetDetails = context.BudgetDetail.Include(p => p.IdArticleNavigation).Where(detail => detail.IdBudget == idBudget).ToList();
            var articles = budgetDetails.Select(details => details.IdArticleNavigation.IdArticleFamily).Distinct().ToList();
            var families = context.ArticleFamily.Where(family => articles.Contains(family.Id)).ToList();
            var countFamily = 0;

            foreach (var family in families) {
                countFamily++;
                var countDetails = 1;
                
                var applicationArticleFamily = new ApplicationArticleFamily {
                    Family = family.Family,
                    Articles = budgetDetails.Select(detail =>
                                                new ApplicationArticle {
                                                    IdArticleFamily = detail.IdArticleNavigation.IdArticleFamily,
                                                    Name = detail.IdArticleNavigation.Name,
                                                    Unit = detail.QuantityUnits,
                                                    Price = detail.UnitPrice,
                                                    TotalPrice = detail.QuantityUnits * detail.UnitPrice
                                                }).Where(article => article.IdArticleFamily == family.Id).ToList()

                };

                applicationArticleFamily.Price = applicationArticleFamily.Articles.Sum(article => article.TotalPrice);
                applicationArticleFamily.Articles.ForEach(article => article.Number = string.Format("{0}.{1}", countFamily, countDetails++));
                applicationArticleFamily.Number = countFamily.ToString();
                articlesByFamily.Add(applicationArticleFamily);

            };

            return articlesByFamily;
        }

        private async Task<string> GetAnagramAsHtmlAsync(IRequestResponse<ReadPlanGeneralDataResponseBase> dataResponse, GeneratePlanRequest request) {
            var anagram = await mediator.Send(new DownloadAnagramRequest {
                PlanId = request.PlanId,
                FilesId = dataResponse.Value.PlanInformation.GeneralData.Anagrams.Select(x => x.Id).ToList(),
                DefaultFile = dataResponse.Value.PlanInformation.GeneralData.Anagrams.All(x => x.DefaultFile is true),
            }).ConfigureAwait(true);

            if (anagram.Status == RequestStatus.Ok) {
                var imageBytesArrayList = anagram.Value.Files.Select(x => x.FileData).ToList();
                string base64;
                //string result = "<div style=\"content:\"\"; clear: both; display: table;\" >";
                string result = "<div style=\"clear: both; display: table;\" >";
                double width = 100 / imageBytesArrayList.Count;
                foreach (var bytes in imageBytesArrayList) {
                    base64 = Convert.ToBase64String(bytes, 0, bytes.Length);
                    MemoryStream ImageStream = new MemoryStream(bytes);
                    if (ImageStream != null) {
                        Bitmap image = new Bitmap(ImageStream);
                        if (image != null) {
                            int widthImage = image.Width;
                            int heightImage = image.Height;
                            if (heightImage > 0) {
                                double imageRelation = (double)widthImage / heightImage;
                                int definitiveWidth = (int)(imageRelation * ImageHeigthConstant < ImageWidthConstant ? imageRelation * ImageHeigthConstant : ImageWidthConstant);
                                result += $"<div style=\"float: left; width: {width} %; padding: 5px; \"><img style=\"width:{definitiveWidth}px; height:{ImageHeigthConstant}px;\" src=\"data:image/jpeg;base64,{ base64 }\" /></div>";
                            }
                        }
                    }
                }
                return result + "</div>";
            }
            return "";
        }

        private Task ApplyOrderByNumber(DocumentContent planTemplateData) {
            planTemplateData.RisksAndPreventiveMeasures
                .OrderBy(p => p.ChapterNumber)
                .ToList();

            planTemplateData.ChaptersHtml
                .OrderBy(p => p.Number)
                .ToList();

            return Task.CompletedTask;
        }

        private async Task FillBlueprintsAsync(int id, DocumentContent planTemplateData) {
            string html = "";
            string htmlIndex = "";
            var response = await mediator.Send(new ViewPlanPlanesRequest { PlanId = id, OnlySelected = true }).ConfigureAwait(true);
            var dataBlueprints = mapper.Map<List<SafetyPlanPlaneDocumentDto>>(response.Value.PlanPlaneList.OrderBy(x => x.Position).ToList());
            IRequestResponse<ViewPlanPlaneFileResponse> result;

            foreach (var dataBlueprint in dataBlueprints) {
                result = await mediator.Send(new ViewPlanPlaneFileRequest {
                    GenericPlaneId = dataBlueprint.IdPlane,
                    CreatedPlanId = dataBlueprint.Id,
                    IsFromGenericBlueprint = dataBlueprint.IsAvailable

                }).ConfigureAwait(true);

                if (result.Status == RequestStatus.Ok) {
                    dataBlueprint.Data = mapper.Map<List<ViewPlanPlaneItemDocumentDto>>(result.Value.files);

                    html += $"<p>{dataBlueprint.Description}<br/>";
                    htmlIndex += $"<p>{dataBlueprint.Description}</p>";
                    foreach (var blueprint in dataBlueprint.Data) {
                        if (blueprint.data.Length != 0)
                            html += $"<img src=\"data:image/jpeg;base64,{ Convert.ToBase64String(blueprint.data, 0, blueprint.data.Length) }\" style=\"width: 500px;\"/>";
                    }

                    html += "<br/></p>";
                }
            }
            planTemplateData.BlueprintsHtml = html;
            planTemplateData.BlueprintsIndexHtml = htmlIndex;
        }

        private void SetIndexAndCheckHtmlTags(DocumentContent planTemplateData) {
            var chapterIndex = 1;
            foreach (var chapter in planTemplateData.ChaptersHtml) {
                var subchapterIndex = 1;
                if (chapter.WordDescriptionHtml != "" && !chapter.WordDescriptionHtml.Contains("<p>") && !chapter.WordDescriptionHtml.Contains("</p>")) {
                    chapter.WordDescriptionHtml = $"<p>{chapter.WordDescriptionHtml}</p>";
                }
                foreach (var subchapter in chapter.SubChaptersHtml) {
                    var activityIndex = 1;
                    if (subchapter.WordDescriptionHtml != "" && !subchapter.WordDescriptionHtml.Contains("<p>") && !subchapter.WordDescriptionHtml.Contains("</p>")) {
                        subchapter.WordDescriptionHtml = $"<p>{subchapter.WordDescriptionHtml}</p>";
                    }
                    foreach (var activities in subchapter.ActivitiesHtml) {
                        if (activities.WordDescriptionHtml != "" && !activities.WordDescriptionHtml.Contains("<p>") && !activities.WordDescriptionHtml.Contains("</p>")) {
                            activities.WordDescriptionHtml = $"<p>{activities.WordDescriptionHtml}</p>";
                        }
                        activities.ActivityIndex = Convert.ToString(chapterIndex) + '.' + Convert.ToString(subchapterIndex) + '.' + Convert.ToString(activityIndex) + '.';
                        activityIndex++;
                    }
                    subchapter.SubchapterIndex = Convert.ToString(chapterIndex) + '.' + Convert.ToString(subchapterIndex) + '.';
                    subchapterIndex++;
                }
                chapter.ChapterIndex = Convert.ToString(chapterIndex) + '.';
                chapterIndex++;
            }
        }

        private void FillRenderData(DocumentContent planTemplateData) {

            foreach (var chapter in planTemplateData.ChaptersHtml) {
                foreach (var subChapter in chapter.SubChaptersHtml) {
                    subChapter.BlueHeaderTableRender = !string.IsNullOrEmpty(subChapter.WorkDetailsHtml) && !string.IsNullOrWhiteSpace(subChapter.WorkDetailsHtml);
                    foreach (var activities in subChapter.ActivitiesHtml) {
                        activities.ActivityRisksRender = activities.MeasuresPerRiskAndActivityHtml.Any();
                        activities.HasWordDescription = activities.WordDescriptionHtml.Any();
                        activities.HasWorkDetails = activities.WorkDetailsHtml.Any();
                        if (chapter.ActivityRisksRender == false) {
                            chapter.ActivityRisksRender = activities.MeasuresPerRiskAndActivityHtml.Any();
                        }
                        activities.AssociatedDetailsRender = !string.IsNullOrEmpty(activities.AssociatedDetails);
                        chapter.WorkDescriptionRender = !string.IsNullOrEmpty(chapter.WordDescriptionHtml)
                            ||
                            !string.IsNullOrEmpty(subChapter.WordDescriptionHtml)
                            ||
                            !string.IsNullOrEmpty(activities.WordDescriptionHtml);
                    }

                }
            }
        }

        private void FillRisksPerActivityData(DocumentContent planTemplateData, string templateName) {

            foreach (var chapter in planTemplateData.ChaptersHtml) {
                foreach (var subChapter in chapter.SubChaptersHtml) {
                    List<PreventiveMeasureListDocumentDto> preventiveMeasureBase = new List<PreventiveMeasureListDocumentDto>();
                    foreach (var act in subChapter.ActivitiesHtml) {

                        var preventiveMeasures = planTemplateData.RisksAndPreventiveMeasures
                                .Where(x => x.ChapterId == chapter.Id && x.SubChapterId == subChapter.Id && act.Id == x.ActivityId).ToList();

                        if (templateName == FORMATO_SIN_TABLA_COMPLETO) {

                            foreach (var preventiveMeasure in preventiveMeasures) {
                                preventiveMeasure.PreventiveMeasures.RemoveAll(x => preventiveMeasureBase.Select(y => y.Id).Contains(x.Id));
                                preventiveMeasureBase.AddRange(preventiveMeasure.PreventiveMeasures);
                            }
                        }

                        act.MeasuresPerRiskAndActivityHtml = mapper.Map<List<MeasuresPerRiskAndActivity>>(preventiveMeasures);

                        act.MeasuresPerRiskAndActivityHtml = act.MeasuresPerRiskAndActivityHtml
                                                                .OrderBy(x => x.RiskOrder)
                                                                .ThenBy(x => x.RiskName)
                                                                .ToList();

                        if (act.MeasuresPerRiskAndActivityHtml.Any())
                            act.MeasuresPerRiskAndActivityHtml[0].WordDescriptionHtml = act.WordDescriptionHtml;
                        //ActivityRisks
                        act.ActivityRisks = mapper.Map<List<PlanFormatoSinTablasActivityRisks>>(planTemplateData.RisksAndPreventiveMeasures
                            .Where(x => x.ChapterId == chapter.Id && x.SubChapterId == subChapter.Id && act.Id == x.ActivityId).OrderBy(x=>x.RiskOrder).ToList());
                    }
                }
            }
        }

        private string EmptyRepeated(string last, string current, bool isFirstElement, string lastFather = "", string currentFather = "") {
            if (isFirstElement) {
                return current;
            }

            if (lastFather == currentFather) {
                return last == current ? string.Empty : current;
            } else {
                return current;
            }
        }

        private void CleanNulls(DocumentContent planTemplateData) {
            if (planTemplateData.AffectedServicesDescriptionHtml is null)
                planTemplateData.AffectedServicesDescriptionHtml = string.Empty;
            if (!planTemplateData.RisksAndPreventiveMeasures.Any()) {
                planTemplateData.RisksAndPreventiveMeasures = new List<RiskAndPreventiveMeasuresDocumentDto>();
                planTemplateData.RisksAndPreventiveMeasuresRender = false;

            }
            if (planTemplateData.ProjectName is null)
                planTemplateData.ProjectName = "";
            if (planTemplateData.CreatorName is null)
                planTemplateData.CreatorName = "";
            if (planTemplateData.ApproverName is null)
                planTemplateData.ApproverName = "";
            if (planTemplateData.WorkLocation is null)
                planTemplateData.WorkLocation = "";
            if (planTemplateData.WorkDescriptionHtml is null)
                planTemplateData.WorkDescriptionHtml = "";
            if (planTemplateData.AffectedServicesDescriptionHtml is null)
                planTemplateData.AffectedServicesDescriptionHtml = "";
            if (!planTemplateData.ChaptersHtml.Any())
                planTemplateData.ChaptersHtml = new List<PlanChapterDocumentDto>();


        }

        private bool isFullFilled(DocumentContent planTemplateData) {
            if (planTemplateData.RisksAndPreventiveMeasures.Any() && planTemplateData.PlanId != 0 && !string.IsNullOrEmpty(planTemplateData.ProjectName) &&
                !string.IsNullOrEmpty(planTemplateData.Date) && !string.IsNullOrEmpty(planTemplateData.CreatorName) && !string.IsNullOrEmpty(planTemplateData.ApproverName) &&
                !string.IsNullOrEmpty(planTemplateData.WorkLocation) && !string.IsNullOrEmpty(planTemplateData.WorkDescriptionHtml) && !string.IsNullOrEmpty(planTemplateData.AffectedServicesDescriptionHtml) &&
                planTemplateData.ChaptersHtml.Any() && planTemplateData.ExecutionTimeMonths != 0 && planTemplateData.ExecutionBudget != 0 &&
                planTemplateData.WorkersNumber != 0
            ) {
                return true;
            }
            return false;
        }

        private async Task<List<RiskAndPreventiveMeasuresDocumentDto>> GetPreventiveMeasures(List<PlanChapterDocumentDto> activities) {
            List<RiskAndPreventiveMeasuresDocumentDto> response = await context.RisksAndPreventiveMeasures
                               .ProjectTo<RiskAndPreventiveMeasuresDocumentDto>(mapper.ConfigurationProvider)
                                .Where(x => activities.Any(chap => chap.Id == x.ChapterId)).ToListAsync();
            foreach (var riskAndMeasure in response) {
                riskAndMeasure.PreventiveMeasures = riskAndMeasure.PreventiveMeasures.OrderBy(x => x.PreventiveMeasureOrder).ToList();
            }
            if (!response.Any()) {
                return new List<RiskAndPreventiveMeasuresDocumentDto>();
            }

            return response;
        }

        private async Task<List<PlanChapterDocumentDto>> GetChaptersAsync(GeneratePlanRequest request) {
            List<SelectedPlanActivity> selList = new List<SelectedPlanActivity>();

            var response = await mediator.Send(new ViewPlanActivitiesRequest {

                PlanId = request.PlanId,
                GetAll = false

            }).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                selList = response.Value.ActivityLists.PlanActivities;
            }

            #region GetOrder
            var chapterOrderList = new List<int>();
            var subchapterOrderList = new List<int>();
            var actOrderList = new List<int>();
            foreach (var chap in request.SelectedActivities) {
                if (!chapterOrderList.Contains(1)) {
                    chapterOrderList.Add(1);
                }
                if (!chapterOrderList.Contains(chap.Id)) {
                    chapterOrderList.Add(chap.Id);
                }
                foreach (var subchapt in chap.SubChapter) {
                    if (!subchapterOrderList.Contains(subchapt.Id)) {
                        subchapterOrderList.Add(subchapt.Id);
                    }
                    foreach (var act in subchapt.Activities) {
                        if (!actOrderList.Contains(act.Id)) {
                            actOrderList.Add(act.Id);
                        }
                    }
                }
            }
            #endregion

            List<int> chapterIds = response.Value.ActivityLists.PlanActivities.Select(x => x.IdActivityVersion).ToList();
            List<int> relationsDone = new List<int>();
            List<int> activityVersionsDone = new List<int>();

            var actVersion = await context.ActivityVersion
               .Include(x => x.IdActivityNavigation)
               .Where(x => chapterIds.Any(y => y == x.Id)).ToListAsync();
            actVersion = actVersion.OrderBy(x => actOrderList.IndexOf(x.IdActivity)).ToList();

            while(NeedActivityRelation == true) {

                if(ActivityRelationList.Count > 0) {
                    List<int> needToSearchActversions = ActivityRelationList;
                    if (activityVersionsDone.Count > 0) {
                        needToSearchActversions = ActivityRelationList.Except(activityVersionsDone).ToList();
                    }

                    var relationalActVersion = await context.ActivityVersion
                        .Include(x => x.IdActivityNavigation)
                        .Where(x => needToSearchActversions.Contains(x.IdActivity)).ToListAsync();

                    activityVersionsDone = activityVersionsDone.Concat(relationalActVersion.Select(x => x.IdActivity).ToList()).Distinct().ToList();

                    foreach (var act in relationalActVersion) {
                        if (!actVersion.Exists(x => x.IdActivity == act.IdActivity))
                            actVersion.Add(act);
                    }
                }

                NeedActivityRelation = false;
                foreach (var act in actVersion) {
                    if (act.RelationsId != null && !relationsDone.Contains((int)act.RelationsId)) {
                        NeedActivityRelation = true;
                        relationsDone.Add((int)act.RelationsId);
                        var activityRelations = await context.ActivityRelations.Where(x => x.IdRelations == act.RelationsId).ToListAsync();

                        foreach (var activityRelation in activityRelations) {

                            if (activityRelation.IdChapterRelation != 0 && !ChapterRelationList.Contains((int)activityRelation.IdChapterRelation)) {
                                ChapterRelationList.Add((int)activityRelation.IdChapterRelation);

                                var currentChapterVersion = await context.ChapterVersion
                                         .Where(x => x.IdChapter == activityRelation.IdChapterRelation && x.ApprovementDate < DateTime.Now && x.EndDate == null || x.EndDate > DateTime.Now)
                                         .FirstOrDefaultAsync();

                                var subchapterIdList = await context.SubChapterVersion
                                         .Where(x => x.IdChapterVersion == currentChapterVersion.Id)
                                         .Select(y => y.IdSubChapter)
                                         .ToListAsync();

                                var activityList = await context.Activity
                                         .Where(x => subchapterIdList.Contains(x.SubChapterId))
                                         .Select(y => y.Id)
                                         .ToListAsync();

                                ActivityRelationList = ActivityRelationList.Concat(activityList).Distinct().ToList();
                            }

                            if (activityRelation.IdSubchapterRelation != 0 && !SubchapterRelationList.Contains((int)activityRelation.IdSubchapterRelation)) {
                                SubchapterRelationList.Add((int)activityRelation.IdSubchapterRelation);

                                var activityList = await context.Activity
                                         .Where(x => x.SubChapterId == (int)activityRelation.IdSubchapterRelation)
                                         .Select(y => y.Id)
                                         .ToListAsync();

                                ActivityRelationList = ActivityRelationList.Concat(activityList).Distinct().ToList();
                            }

                            if (activityRelation.IdActivityRelation != 0 && !ActivityRelationList.Contains((int)activityRelation.IdActivityRelation)) {
                                ActivityRelationList.Add((int)activityRelation.IdActivityRelation);
                            }
                        }
                    }
                }
            }

            List<int> subChapterVarsionIds = actVersion.Select(x => x.IdSubChapterVersion).Distinct().ToList();

            List<SubChapterVersion> subChaptVersion = await context.SubChapterVersion
                .Where(x => subChapterVarsionIds.Any(id => id == x.Id) || (x.IdChapterVersionNavigation.Number == 1 && !x.ActivityVersion.Any()))
                .Include(x => x.IdSubChapterNavigation)
                .ToListAsync();
            subChaptVersion = subChaptVersion.OrderBy(x => subchapterOrderList.IndexOf(x.IdSubChapter)).ToList();

            List<int> chapterVersionIds = subChaptVersion.Select(x => x.IdChapterVersion).Distinct().ToList();

            List<ChapterVersion> chapterVersion = await context.ChapterVersion
                .Where(x => chapterVersionIds.Any(id => id == x.Id))
                .Include(x => x.IdChapterNavigation)
                .OrderBy(x => x.Number)
                .ToListAsync();

            var selectedChapterVersion = chapterVersion.Where(x => chapterOrderList.Contains(x.IdChapter))
                .OrderBy(x => chapterOrderList.IndexOf(x.IdChapter))
                .ToList();
            chapterVersion = selectedChapterVersion.Concat(chapterVersion).Distinct().ToList();

            SelectedPlanActivity selectedPlanActivity;
            foreach (var chapt in chapterVersion) {
                chapt.SubChapterVersion = subChaptVersion.Where(x => x.IdChapterVersion == chapt.Id).ToList();
                if (chapt.IdChapter == 1 || !chapterOrderList.Contains(chapt.IdChapter)) {
                    chapt.SubChapterVersion = chapt.SubChapterVersion.OrderBy(x => x.Number).ToList();
                } else {
                    //Mantener los caps seleccionados los primeros y dejar despues los que vengan de actividades relacionadas por orden numerico
                    var subchapterVersionSelected = chapt.SubChapterVersion.Where(x => subchapterOrderList.Contains(x.IdSubChapter)).ToList();
                    var subchapterVersionRemain = chapt.SubChapterVersion.Except(subchapterVersionSelected).OrderBy(x => x.Number).ToList();
                    chapt.SubChapterVersion = subchapterVersionSelected.Concat(subchapterVersionRemain).ToList();
                }

                foreach (var subChapter in chapt.SubChapterVersion) {
                    subChapter.ActivityVersion = actVersion.Where(x => x.IdSubChapterVersion == subChapter.Id).ToList();
                    if(chapt.IdChapter == 1 || !chapterOrderList.Contains(chapt.IdChapter)) {
                        subChapter.ActivityVersion = subChapter.ActivityVersion.OrderBy(x => x.Number).ToList();
                    }

                    foreach (var act in subChapter.ActivityVersion) {
                        selectedPlanActivity = response.Value.ActivityLists.PlanActivities.FirstOrDefault(x => x.IdActivityVersion == act.Id);

                        if (selectedPlanActivity != null) {
                            chapt.WordDescription = String.IsNullOrEmpty(selectedPlanActivity.ChapterDescription) ? chapt.WordDescription : selectedPlanActivity.ChapterDescription;
                            subChapter.Description = String.IsNullOrEmpty(selectedPlanActivity.SubChapterDescription) ? subChapter.Description : selectedPlanActivity.SubChapterDescription;
                            act.WordDescription = String.IsNullOrEmpty(selectedPlanActivity.WordDescription) ? act.WordDescription : selectedPlanActivity.WordDescription;
                        }
                    }
                }
            }
            foreach(var chapter in request.SelectedActivities) {
                if (!chapterVersion.Any(x=>x.IdChapter==chapter.Id) || chapterVersion.Any(x => x.IdChapter == chapter.Id) && !chapterVersion.Any(x => x.Title == chapter.Title)) {
                    var chapterCustom = mapper.Map<ChapterVersion>(chapter);
                    if (chapter.Position - 1>= chapterVersion.Count) {
                        chapterVersion.Add(chapterCustom);
                    } else {
                        chapterVersion.Insert(chapter.Position - 1, chapterCustom);
                    }
                } 
                else {
                    foreach (var subchapter in chapter.SubChapter) {
                        if (!chapterVersion.Where(z=>z.IdChapter==chapter.Id).Any(x => x.SubChapterVersion.Any(c => c.IdSubChapter == subchapter.Id)) || chapterVersion.Where(z => z.IdChapter == chapter.Id).Any(x => x.SubChapterVersion.Any(c => c.IdSubChapter == subchapter.Id)) && !chapterVersion.Where(z => z.IdChapter == chapter.Id).Any(x => x.SubChapterVersion.Any(c => c.Title == subchapter.Title))) {
                            var subchapterCustom = mapper.Map<SubChapterVersion>(subchapter);
                            chapterVersion.Where(v => v.IdChapter == chapter.Id).SelectMany(x => x.SubChapterVersion).ToList().Insert(subchapter.Position - 1, subchapterCustom);
                            if (subchapter.Position - 1 >= chapterVersion.Where(v => v.IdChapter == chapter.Id).SelectMany(x => x.SubChapterVersion).ToList().Count) {
                                chapterVersion.Where(v => v.IdChapter == chapter.Id).FirstOrDefault().SubChapterVersion.Add(subchapterCustom);
                            } else {
                                var subChapterlist = chapterVersion.Where(v => v.IdChapter == chapter.Id).FirstOrDefault().SubChapterVersion.ToList();
                                subChapterlist.Insert(subchapter.Position - 1, subchapterCustom);
                                chapterVersion.Where(v => v.IdChapter == chapter.Id).FirstOrDefault().SubChapterVersion = subChapterlist;
                            }
                        } else {
                            foreach (var activities in subchapter.Activities) {
                                if (!chapterVersion.Where(z => z.IdChapter == chapter.Id).Any(x => x.SubChapterVersion.Where(z => z.IdSubChapter == subchapter.Id).Any(c => c.ActivityVersion.Any(a=>a.IdActivity == activities.Id))) || chapterVersion.Where(z => z.IdChapter == chapter.Id).Any(x => x.SubChapterVersion.Where(z => z.IdSubChapter == subchapter.Id).Any(c => c.ActivityVersion.Any(a => a.IdActivity == activities.Id))) && !chapterVersion.Where(z => z.IdChapter == chapter.Id).Any(x => x.SubChapterVersion.Where(z => z.IdSubChapter == subchapter.Id).Any(c => c.ActivityVersion.Any(a => a.Description == activities.Description)))) {
                                    var activityCustom = mapper.Map<ActivityVersion>(activities);
                                    if (activities.Position - 1 >= chapterVersion.Where(v => v.IdChapter == chapter.Id).SelectMany(x => x.SubChapterVersion).Where(z => z.IdSubChapter == subchapter.Id).SelectMany(c => c.ActivityVersion).ToList().Count) {
                                        chapterVersion.Where(v => v.IdChapter == chapter.Id).FirstOrDefault().SubChapterVersion.Where(z=>z.IdSubChapter == subchapter.Id)
                                            .FirstOrDefault().ActivityVersion
                                            .Add(activityCustom);
                                    } else {
                                        var Activitieslist = chapterVersion.Where(v => v.IdChapter == chapter.Id).FirstOrDefault().SubChapterVersion.Where(z => z.IdSubChapter == subchapter.Id)
                                            .FirstOrDefault().ActivityVersion.ToList();
                                        Activitieslist.Insert(activities.Position - 1, activityCustom);
                                        chapterVersion.Where(v => v.IdChapter == chapter.Id).FirstOrDefault().SubChapterVersion.Where(z => z.IdSubChapter == subchapter.Id)
                                            .FirstOrDefault().ActivityVersion = Activitieslist;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return mapper.Map<List<PlanChapterDocumentDto>>(chapterVersion);
        }
    }
}
