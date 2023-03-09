using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.AllDocuments.Documents;
using Segurplan.Core.Actions.AllDocuments.Helpers;
using Segurplan.Core.Actions.AllDocuments.Models;
using Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List.Specifications;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.Core.Actions.RiskEvaluation.EvaluationsOfRisksAndPreventiveMeasures.Generate.Models;
using Segurplan.Core.Database;
using Segurplan.Core.Domain.CacheServices;
using Segurplan.Core.Domain.Documents;
using Segurplan.Core.Helpers;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.RiskEvaluation.EvaluationsOfRisksAndPreventiveMeasures.Generate.RiskMap {
    public class GenerateEvaluationOfRisksDocsRequestHanlder : IRequestHandler<GenerateEvaluationOfRisksDocsRequest, IRequestResponse<GenerateEvaluationOfRisksDocsRequestResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly UserManager<User> userManager;

        public GenerateEvaluationOfRisksDocsRequestHanlder(SegurplanContext context, IMapper mapper, IMediator mediator, UserManager<User> userManager) {
            this.context = context;
            this.mapper = mapper;
            this.mediator = mediator;
            this.userManager = userManager;
        }

        private GenerateRiskEvaluationModel GenerateRiskEvaluationModel;
        private List<int> SubchapterIdList;
        private List<int> ActivityIdList;

        public async Task<IRequestResponse<GenerateEvaluationOfRisksDocsRequestResponse>> Handle(GenerateEvaluationOfRisksDocsRequest request, CancellationToken cancellationToken) {

            SubchapterIdList = request.FilterData.Select(x => x.SubChapterId).Distinct().ToList();
            ActivityIdList = request.FilterData.Select(x => x.ActivityId).Distinct().ToList();

            if (!string.IsNullOrEmpty(request.TargetTemplate)) {

                await FillGenerateRiskEvaluationModel(request);
                DocumentContent documentContent = new DocumentContent();
                List<ProcesedDocument> procesedDocuments = new List<ProcesedDocument>();
                bool zipResponseNedded = string.IsNullOrEmpty(request.Title);

                GenerateRiskEvaluationModel.planChapterDto.OrderBy(x => x.Id);
                foreach (var chapter in GenerateRiskEvaluationModel.planChapterDto) {
                    if (zipResponseNedded) {
                        request.Title = chapter.Title + ".docx";
                        documentContent.DocumentTitle = chapter.Title;

                    } else {
                        documentContent.DocumentTitle = request.Title;
                    }

                    documentContent.ChaptersHtml.Add(chapter);
                    documentContent.RisksAndPreventiveMeasures = GenerateRiskEvaluationModel.riskAndPreventiveMeasuresDto.Where(x => x.ChapterId == chapter.Id).ToList();
                    if (zipResponseNedded) {
                        SetNumber(documentContent);
                    }
                    FillRisksPerActivityData(chapter);
                    FillRenderData(documentContent);

                    if (zipResponseNedded) {
                        CustomBusinessMappings.FillRisksPerActivityCharsData(documentContent);
                        await GetDocument(documentContent, request, procesedDocuments, chapter, ".docx");
                        documentContent.ChaptersHtml = new List<PlanChapterDocumentDto>();
                    }
                }

                for (int i = 0; documentContent.ChaptersHtml.Count < i; i++) {
                    documentContent.ChaptersHtml[i].SubChaptersHtml.OrderBy(x => x.Number);
                }

                
                SetIndex(documentContent);
               
                if (!zipResponseNedded) {
                    CustomBusinessMappings.FillRisksPerActivityCharsData(documentContent);
                    await GetDocument(documentContent, request, procesedDocuments, documentContent.ChaptersHtml[0]);
                }


                if (procesedDocuments.Count == 0)
                    return RequestResponse.NotFound<GenerateEvaluationOfRisksDocsRequestResponse>();


                if (procesedDocuments.Count > 1) {
                    var zip = ZipBuilder.ToZipMemoryStreamRange(procesedDocuments);

                    return RequestResponse.Ok(new GenerateEvaluationOfRisksDocsRequestResponse(
                      zip.Item1,
                      "documentos" + zip.Item3,
                      zip.Item2
                      ));

                } else {
                    return RequestResponse.Ok(new GenerateEvaluationOfRisksDocsRequestResponse(
                        procesedDocuments.First().ResponseStream,
                        procesedDocuments.First().OutputFileName/* + ".docx"*/,
                        procesedDocuments.First().MediaType
                        ));
                }
            }
            return RequestResponse.NotFound<GenerateEvaluationOfRisksDocsRequestResponse>();
        }

        private void FillRenderData(DocumentContent documentContent) {
            foreach (var chapter in documentContent.ChaptersHtml) {
                chapter.SubChaptersHtml = chapter.SubChaptersHtml.OrderBy(x => x.Number).ToList();
                foreach (var subChapter in chapter.SubChaptersHtml) {
                    subChapter.BlueHeaderTableRender = !string.IsNullOrEmpty(subChapter.WorkDetailsHtml) && !string.IsNullOrWhiteSpace(subChapter.WorkDetailsHtml);
                    foreach (var activities in subChapter.ActivitiesHtml) {
                        activities.ActivityRisksRender = activities.MeasuresPerRiskAndActivityHtml.Any();
                        activities.HasWordDescription = activities.WordDescriptionHtml.Any();
                        activities.HasWorkDetails = activities.WorkDetailsHtml.Any();
                        if(chapter.ActivityRisksRender == false) {
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
        private void SetNumber(DocumentContent documentContent) {
            
            foreach (var chapter in documentContent.ChaptersHtml) {
                chapter.SubChaptersHtml = chapter.SubChaptersHtml.OrderBy(x => SubchapterIdList.IndexOf(x.Id)).ToList();
                var subchapterIndex = 1;
                foreach (var subchapter in chapter.SubChaptersHtml) {
                    subchapter.ActivitiesHtml = subchapter.ActivitiesHtml.OrderBy(x => ActivityIdList.IndexOf(x.Id)).ToList();
                    var activityIndex = 1;
                    foreach (var activities in subchapter.ActivitiesHtml) {
                        activities.Number = activityIndex;
                        activityIndex++;
                    }
                    subchapter.Number = subchapterIndex;
                    subchapterIndex++;
                }
            }
        }
        private void SetIndex(DocumentContent documentContent) {
            var chapterIndex = 1;
            foreach (var chapter in documentContent.ChaptersHtml) {
                chapter.SubChaptersHtml = chapter.SubChaptersHtml.OrderBy(x => SubchapterIdList.IndexOf(x.Id)).ToList();
                var subchapterIndex = 1;
                foreach (var subchapter in chapter.SubChaptersHtml) {
                    subchapter.ActivitiesHtml = subchapter.ActivitiesHtml.OrderBy(x => ActivityIdList.IndexOf(x.Id)).ToList();
                    var activityIndex = 1;
                    foreach (var activities in subchapter.ActivitiesHtml) {
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

        private void FillRisksPerActivityData(PlanChapterDocumentDto chapter) {
            List<MeasuresPerRiskAndActivity> itemsToAdd = new List<MeasuresPerRiskAndActivity>();
            List<PreventiveMeasureListDocumentDto> allMeasures = new List<PreventiveMeasureListDocumentDto>();

            foreach (var subChapter in chapter.SubChaptersHtml) {
                subChapter.ActivitiesHtml = subChapter.ActivitiesHtml.OrderBy(x => x.Number).ToList(); 
                foreach (var act in subChapter.ActivitiesHtml) {
                    act.MeasuresPerRiskAndActivityHtml = mapper.Map<List<MeasuresPerRiskAndActivity>>(GenerateRiskEvaluationModel.riskAndPreventiveMeasuresDto
                        .Where(x => x.ChapterId == chapter.Id && x.SubChapterId == subChapter.Id && act.Id == x.ActivityId).ToList());
                    act.MeasuresPerRiskAndActivityHtml = act.MeasuresPerRiskAndActivityHtml.OrderBy(x => x.RiskOrder).ToList();
                }
                
            }

        }

        private async Task GetDocument(DocumentContent documentContent, GenerateEvaluationOfRisksDocsRequest request, List<ProcesedDocument> procesedDocuments, PlanChapterDocumentDto chapter, string extToAdd = null) {
            FillAuditDataAsync(documentContent, chapter);
            var fileResponse = await mediator.Send(new CreateDocumentRequest(documentContent, request.TargetTemplate, request.Title)).ConfigureAwait(false);
            fileResponse.Value.Document.OutputFileName = documentContent.DocumentTitle;

            if (!string.IsNullOrEmpty(extToAdd) && !fileResponse.Value.Document.OutputFileName.Contains(".docx")) {
                fileResponse.Value.Document.OutputFileName += extToAdd;
            }
            procesedDocuments.Add(fileResponse.Value.Document);
        }

        private void FillAuditDataAsync(DocumentContent planTemplateData, PlanChapterDocumentDto chapter) {

            planTemplateData.CheckDate = chapter.ApprovementDate.HasValue ? chapter.ApprovementDate.Value.ToString("dd/MM/yyyy") : null;
            planTemplateData.CreateDate = chapter.CreateDate.ToString("dd/MM/yyyy");
            planTemplateData.ChapterVersion = chapter.VersionNumber;
            string userId = null;
            var versionId = context.Set<ChapterVersion>().Where(x => x.Title == chapter.Title && x.VersionNumber == chapter.VersionNumber).Select(x => x.Id).First();

            if (context.Set<UserChapterVersion>().Where(x => x.ChapterVersionId == versionId).Any())
                userId = context.Set<UserChapterVersion>().Where(x => x.ChapterVersionId == versionId).Select(x => x.UserId).First().ToString();

            if (userId != null)
                planTemplateData.CreatorName = userManager.FindByIdAsync(userId).Result.CompleteName;
            else
                planTemplateData.CreatorName = "";

            if (chapter.IdApprover != default)
                planTemplateData.ApproverName = userManager.FindByIdAsync(chapter.IdApprover.ToString()).Result.CompleteName;
            else
                planTemplateData.ApproverName = "";

            if (chapter.IdReviewer != default)
                planTemplateData.RevisorName = userManager.FindByIdAsync(chapter.IdReviewer.ToString()).Result.CompleteName;
            else
                planTemplateData.RevisorName = "";
        }

        private async Task FillGenerateRiskEvaluationModel(GenerateEvaluationOfRisksDocsRequest request) {
            var riskAndPreventiveMeasures = await GetPreventiveMeasures(request);
            foreach(var riskAndMeasure in riskAndPreventiveMeasures) {
                riskAndMeasure.PreventiveMeasures = riskAndMeasure.PreventiveMeasures.OrderBy(x => x.PreventiveMeasureOrder).ToList();
            }
            GenerateRiskEvaluationModel = new GenerateRiskEvaluationModel(
              riskAndPreventiveMeasures, await GetChaptersByMeasures(request, riskAndPreventiveMeasures));
        }

        private async Task<List<PlanChapterDocumentDto>> GetChaptersByMeasures(GenerateEvaluationOfRisksDocsRequest request, List<RiskAndPreventiveMeasuresDocumentDto> risksAndPreventiveMeasures) {
            List<PlanChapterDocumentDto> chapters;

            if (!request.FilterData.Any() && risksAndPreventiveMeasures.Any()) {
                GenerateFilterDataFromPreventiveMeasures(risksAndPreventiveMeasures, request);
            }

            var chapterOrder = new List<int>();
            foreach (var item in request.FilterData) {
                if (chapterOrder.Contains(item.ChapterId)) {

                } else {
                    chapterOrder.Add(item.ChapterId);
                }
            }

            if (request.FilterData.Any() || risksAndPreventiveMeasures.Any()) {
                chapters = await context.ChapterVersion
                        .Where(chapt => request.FilterData.Any(fd => fd.ChapterId == chapt.IdChapter &&
                        (chapt.ApprovementDate < DateTime.Now && chapt.EndDate == null || chapt.EndDate > DateTime.Now)))
                        .ProjectTo<PlanChapterDocumentDto>(mapper.ConfigurationProvider)
                        .ToListAsync();
                chapters = chapters.OrderBy(x => chapterOrder.IndexOf(x.Id)).ToList();
            } else {
                chapters = new List<PlanChapterDocumentDto>();
            }

            chapters.Select(chap => {
                chap.SubChaptersHtml = chap.SubChaptersHtml.Where(x => request.FilterData.Any(fd => fd.SubChapterId == x.Id)).ToList();

                chap.SubChaptersHtml.Select(sub => {
                    if (sub.ActivitiesHtml != null)
                        sub.ActivitiesHtml = sub.ActivitiesHtml.Where(x => request.FilterData.Any(fd => fd.ActivityId == x.Id)).ToList();

                    return sub;
                }).ToList();
                return chap;
            }).ToList();

            return chapters;
        }

        private void GenerateFilterDataFromPreventiveMeasures(List<RiskAndPreventiveMeasuresDocumentDto> risksAndPreventiveMeasures, GenerateEvaluationOfRisksDocsRequest request) {
            request.FilterData = new List<ChaptSubChaptActFilterData>();
            foreach (var item in risksAndPreventiveMeasures) {
                request.FilterData.Add(new ChaptSubChaptActFilterData {
                    ChapterId = item.ChapterId,
                    SubChapterId = item.SubChapterId,
                    ActivityId = item.ActivityId

                });
            }
        }

        private async Task<List<RiskAndPreventiveMeasuresDocumentDto>> GetPreventiveMeasures(GenerateEvaluationOfRisksDocsRequest request) {
            IRequestResponse<ListRisksAndPreventiveMeasuresResponse> response = await mediator.Send(new ListRisksAndPreventiveMeasuresRequest() {
                Specifications = GetSpecificationsAsync(request)
            }).ConfigureAwait(true);

            if (response.Status != RequestStatus.Ok) {
                return new List<RiskAndPreventiveMeasuresDocumentDto>();
            }

            return mapper.Map<List<RiskAndPreventiveMeasuresDocumentDto>>(response.Value.RiskAndPrevMeasures);
        }

        private IEnumerable<ISpecification<ListRisksAndPreventiveMeasuresResponse.ListItem>> GetSpecificationsAsync(GenerateEvaluationOfRisksDocsRequest request) {
            var specifications = new List<ISpecification<ListRisksAndPreventiveMeasuresResponse.ListItem>>();
            var searchFilter = new FilterRisksAndPreventiveMeasuresSpecification();

            if (request.FilterData.Any())
                searchFilter.ChaptSubChaptActIds(request.FilterData);

            specifications.Add(searchFilter);

            return specifications;
        }
    }
}
