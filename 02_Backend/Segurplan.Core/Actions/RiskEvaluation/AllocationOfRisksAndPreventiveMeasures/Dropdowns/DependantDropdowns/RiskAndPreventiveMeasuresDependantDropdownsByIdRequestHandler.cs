using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.DependantDropdowns {
    public class RiskAndPreventiveMeasuresDependantDropdownsByIdRequestHandler : IRequestHandler<RiskAndPreventiveMeasuresDependantDropdownsByIdRequest, IRequestResponse<RiskAndPreventiveMeasuresDependantDropdownsByIdResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public RiskAndPreventiveMeasuresDependantDropdownsByIdRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<RiskAndPreventiveMeasuresDependantDropdownsByIdResponse>> Handle(RiskAndPreventiveMeasuresDependantDropdownsByIdRequest request, CancellationToken cancellationToken) {
            List<SubChapterDropdownDto> subChapter = new List<SubChapterDropdownDto>();
            List<SubChapterDropdownDto> subChapterVersion = new List<SubChapterDropdownDto>();
            List<ActivityDropdownDto> activities = new List<ActivityDropdownDto>();
            List<ChapterVersionDto> chapterVerison = new List<ChapterVersionDto>();
            //List<RiskDropdownDto> risks = new List<RiskDropdownDto>();
            List< PreventiveMeasureDetailDto> preventiveMeasures = new List<PreventiveMeasureDetailDto>();
            var vigente = true;

            switch (request.Target) {
                case "currentSubChapter":
                    var currentChapterVersion = context.ChapterVersion
                                     .Where(x => x.ApprovementDate < DateTime.Now && x.EndDate == null || x.EndDate > DateTime.Now)
                                     .ProjectTo<ChapterVersionDto>(mapper.ConfigurationProvider);

                 

                    var idChapterVersionCurrent = currentChapterVersion.Where(x => x.IdChapter == request.DependantRelationId).Select(x=>x.Id).FirstOrDefault();
                    if (idChapterVersionCurrent == 0) {
                        chapterVerison = await context.ChapterVersion
                                     .Where(x => x.ApprovementDate == null || x.ApprovementDate > DateTime.Now)
                                     .ProjectTo<ChapterVersionDto>(mapper.ConfigurationProvider)
                                     .ToListAsync();
                        idChapterVersionCurrent = chapterVerison.Where(x => x.IdChapter == request.DependantRelationId).Select(x => x.Id).FirstOrDefault();
                    }
                        subChapterVersion = await GetSubchapterVersion(idChapterVersionCurrent);

                    break;
                case "subChapterVersion":
                    subChapterVersion = await GetSubchapterVersion(request.DependantRelationId);
                    break;
                case "actividad":
                    activities = await context.Activity
                                     .Where(x => x.SubChapterId == request.DependantRelationId)
                                     .ProjectTo<ActivityDropdownDto>(mapper.ConfigurationProvider)
                                     .OrderBy(x => x.Number)
                                     .ToListAsync();
                    break;
                case "chapterVersion":
                    chapterVerison = await context.ChapterVersion
                                     .Where(x => x.Id == request.ChapterVersionId)
                                     .ProjectTo<ChapterVersionDto>(mapper.ConfigurationProvider)
                                     .OrderBy(chap => chap.VersionNumber)
                                     .ToListAsync(); 
                    break;
                case "chapterVersionByChapter":
                    var chapterVersionCurrentById = await context.ChapterVersion
                                     .Where(x => x.IdChapter == request.ChapterId && (x.ApprovementDate < DateTime.Now && x.EndDate == null || x.EndDate > DateTime.Now))
                                     .ProjectTo<ChapterVersionDto>(mapper.ConfigurationProvider)
                                     .ToListAsync();

                    var subChapterVersionFromCurrent = chapterVersionCurrentById.Count == 0? new List<SubChapterDropdownDto>() : await context.SubChapterVersion
                                   .Where(x => x.IdChapterVersion == chapterVersionCurrentById[0].Id)
                                   .ProjectTo<SubChapterDropdownDto>(mapper.ConfigurationProvider)
                                   .OrderBy(x => x.Number)
                                   .ToListAsync();

                    var isOnCurrent = subChapterVersionFromCurrent.Find(x => x.IdSubchapter == Convert.ToString(request.SubChapterId));
                    if (isOnCurrent != null) {
                        vigente = true;
                        chapterVerison = chapterVersionCurrentById;
                        subChapterVersion = subChapterVersionFromCurrent;
                    }
                    else{
                        vigente = false;
                        chapterVerison = await context.ChapterVersion
                                     .Where(x => x.ApprovementDate == null || x.ApprovementDate > DateTime.Now)
                                     .ProjectTo<ChapterVersionDto>(mapper.ConfigurationProvider)
                                     .ToListAsync(); 

                        subChapterVersion = await context.SubChapterVersion
                                   .Where(x => x.IdChapterVersion == chapterVerison[0].Id)
                                   .ProjectTo<SubChapterDropdownDto>(mapper.ConfigurationProvider)
                                   .OrderBy(x => x.Number)
                                   .ToListAsync(); 
                    }

                    break;
                case "medida":
                   //risks = await context.Risk
                   //                  .Join(context.RisksAndPreventiveMeasures.Where(x=>x.ActivityId==request.DependantRelationId),x=>x.Id,z=>z.RiskId,(z,x)=>new RiskDropdownDto() {Id = z.Id, Code = z.Code,Name = z.Name })
                   //                  .OrderBy(x => x.Code)
                   //                  .ToListAsync();                   
                    preventiveMeasures = await context.PreventiveMeasure
                                      .Join(context.RiskAndPreventiveMeasuresMeasures
                                      .Join(context.RisksAndPreventiveMeasures.Where(x => x.ActivityId == request.DependantRelationId), x => x.RisksAndPreventiveMeasuresId, z => z.Id, (z, x) => new {Id = z.PreventiveMeasureId}), x=>x.Id,z=>z.Id, (x,z) => new PreventiveMeasureDetailDto { Id = x.Id,Description = x.Description,Code = x.Code.ToString()})
                                      .Distinct()
                                      .OrderBy(x => x.Description)
                                      .ToListAsync();
                    break;
                case "All":
                    subChapter = await context.SubChapter
                                    .Where(x => x.IdChapter == request.ChapterId)
                                    .ProjectTo<SubChapterDropdownDto>(mapper.ConfigurationProvider)
                                    .OrderBy(x => x.Number)
                                    .ToListAsync();
                    activities = await context.Activity
                                     .Where(x => x.SubChapterId == request.SubChapterId)
                                     .ProjectTo<ActivityDropdownDto>(mapper.ConfigurationProvider)
                                     .OrderBy(x => x.Number)
                                     .ToListAsync();
                    subChapterVersion = await GetSubchapterVersion(request.ChapterId);
                    break;
                default:
                    //no target
                    break;
            }


            if (!subChapter.Any() && !activities.Any() && !subChapterVersion.Any() && !chapterVerison.Any() /*&& !risks.Any()*/&&!preventiveMeasures.Any()) {
                return RequestResponse.NotFound<RiskAndPreventiveMeasuresDependantDropdownsByIdResponse>();
            }
            return RequestResponse.Ok(new RiskAndPreventiveMeasuresDependantDropdownsByIdResponse(subChapter, activities, subChapterVersion, chapterVerison, vigente/*,risks*/,preventiveMeasures));
        }


        private async Task<List<SubChapterDropdownDto>> GetSubchapterVersion(int idChapterVersion) {
            return await context.SubChapterVersion
                                   .Where(x => x.IdChapterVersion == idChapterVersion)
                                   .ProjectTo<SubChapterDropdownDto>(mapper.ConfigurationProvider)
                                   .OrderBy(x => x.Number)
                                   .ToListAsync();
        }
    }
}
