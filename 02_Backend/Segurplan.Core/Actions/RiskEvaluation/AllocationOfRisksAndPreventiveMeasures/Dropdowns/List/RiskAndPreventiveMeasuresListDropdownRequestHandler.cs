using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.List {

    public class RiskAndPreventiveMeasuresListDropdownRequestHandler : IRequestHandler<RiskAndPreventiveMeasuresListDropdownRequest, IRequestResponse<RiskAndPreventiveMeasuresListDropdownResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public RiskAndPreventiveMeasuresListDropdownRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<RiskAndPreventiveMeasuresListDropdownResponse>> Handle(RiskAndPreventiveMeasuresListDropdownRequest request, CancellationToken cancellationToken) {
            var chapter = await context.Chapter
                                     .ProjectTo<ChapterDropdownDto>(mapper.ConfigurationProvider)
                                     .OrderBy(chap=>chap.Number)
                                     .ToListAsync();
            var currentChapterVersion = context.ChapterVersion
                                     .Where(x =>x.IdChapter==request.ChapterId &&( x.ApprovementDate < DateTime.Now && x.EndDate == null || x.EndDate > DateTime.Now))
                                     .ProjectTo<ChapterVersionDto>(mapper.ConfigurationProvider)
                                     .FirstOrDefault();
            var borradorChapterVersion = context.ChapterVersion
                                     .Where(x => x.IdChapter == request.ChapterId && (x.ApprovementDate == null || x.ApprovementDate > DateTime.Now))
                                     .ProjectTo<ChapterVersionDto>(mapper.ConfigurationProvider)
                                     .FirstOrDefault();

            var borradorExist = true;

            if (borradorChapterVersion == null)
                borradorExist = false;

            List<SubChapterDropdownDto> subChapterCurrent = new List<SubChapterDropdownDto>();
            List<SubChapterDropdownDto> subChapterBorrador = new List<SubChapterDropdownDto>();
            List<int> subChapterIdList = new List<int>();
            if (request.ChapterId != 0) {
                if (currentChapterVersion!=null) {
                    subChapterCurrent = await context.SubChapterVersion
                                   .Where(x => x.IdChapterVersion == currentChapterVersion.Id)
                                   .ProjectTo<SubChapterDropdownDto>(mapper.ConfigurationProvider)
                                   .OrderBy(x => x.Number)
                                   .ToListAsync();
                }
                if (request.Borrador == false) {
                    if (currentChapterVersion != null) {
                        subChapterIdList = await context.SubChapterVersion
                               .Where(x => x.IdChapterVersion == currentChapterVersion.Id)
                               .ProjectTo<SubChapterDropdownDto>(mapper.ConfigurationProvider)
                               .OrderBy(x => x.Number).Select(x => Convert.ToInt32(x.IdSubchapter))
                               .ToListAsync();
                    }
                } else {
                    if(borradorExist == true) {
                        subChapterBorrador = await context.SubChapterVersion
                                   .Where(x => x.IdChapterVersion == borradorChapterVersion.Id)
                                   .ProjectTo<SubChapterDropdownDto>(mapper.ConfigurationProvider)
                                   .OrderBy(x => x.Number)
                                   .ToListAsync();

                        subChapterIdList = await context.SubChapterVersion
                                   .Where(x => x.IdChapterVersion == borradorChapterVersion.Id)
                                   .ProjectTo<SubChapterDropdownDto>(mapper.ConfigurationProvider)
                                   .OrderBy(x => x.Number).Select(x => Convert.ToInt32(x.IdSubchapter))
                                   .ToListAsync();
                        if (request.SubChapterId == 0)
                            subChapterCurrent = subChapterBorrador;

                    }
                }

            }

            List<ActivityDropdownDto> activityCurrent = new List<ActivityDropdownDto>();
            List<ActivityDropdownDto> activityBorrador = new List<ActivityDropdownDto>();
            //List<RiskDropdownDto> risk = new List<RiskDropdownDto>();
            List<PreventiveMeasureDetailDto> measure = new List<PreventiveMeasureDetailDto>();
            var subChapterId = request.SubChapterId;
            var activityId = 0;

            if (request.SubChapterId != 0) {

                activityCurrent = await context.Activity
                                     .ProjectTo<ActivityDropdownDto>(mapper.ConfigurationProvider)
                                     .Where(p => p.SubChapterId == request.SubChapterId)
                                     .OrderBy(act => act.Number)
                                     .ToListAsync();

                if (request.Borrador == true) {

                    subChapterId = subChapterCurrent.Count() == 0? request.SubChapterId : Convert.ToInt32(subChapterBorrador
                    .Where(x => x.Number == (subChapterCurrent
                        .Where(y => y.IdSubchapter == Convert.ToString(request.SubChapterId))
                        .Select(z => z.Number))
                        .FirstOrDefault())
                    .Select(v => v.IdSubchapter)
                    .FirstOrDefault());
                    if (borradorExist == true)
                        subChapterCurrent = subChapterBorrador;
                }

                if (request.ActivityId != 0) {
                    activityId = request.ActivityId;

                    if (request.Borrador == true) {
                        activityBorrador = await context.Activity
                                     .ProjectTo<ActivityDropdownDto>(mapper.ConfigurationProvider)
                                     .Where(p => p.SubChapterId == Convert.ToInt32(subChapterId))
                                     .OrderBy(act => act.Number)
                                     .ToListAsync();

                        activityId = activityBorrador
                            .Where(x => x.Number == (activityCurrent
                                .Where(y => y.Id == request.ActivityId)
                                .Select(z => z.Number))
                                .FirstOrDefault())
                            .Select(v => v.Id)
                            .FirstOrDefault();
                    }

                    //risk = await context.Risk
                    //                     .Join(context.RisksAndPreventiveMeasures.Where(x => x.ActivityId == request.ActivityId), x => x.Id, z => z.RiskId, (z, x) => new RiskDropdownDto() { Id = z.Id, Code = z.Code, Name = z.Name })
                    //                     .OrderBy(x=>x.Code)
                    //                     .ToListAsync();
                    measure = await context.PreventiveMeasure
                                  .Join(context.RiskAndPreventiveMeasuresMeasures
                                  .Join(context.RisksAndPreventiveMeasures.Where(x => x.ActivityId == request.ActivityId), x => x.RisksAndPreventiveMeasuresId, z => z.Id, (z, x) => new { Id = z.PreventiveMeasureId }), x => x.Id, z => z.Id, (x, z) => new PreventiveMeasureDetailDto { Id = x.Id, Description = x.Description, Code = x.Code.ToString() })
                                  .Distinct()
                                  .OrderBy(x => x.Description)
                                  .ToListAsync();
                }

            }
            var risk = await context.Risk
                                     .ProjectTo<RiskDropdownDto>(mapper.ConfigurationProvider)
                                     .OrderBy(x => x.Code)
                                     .ToListAsync();

            //var measure = await context.RiskAndPreventiveMeasuresMeasures
            //                        .ProjectTo<PreventiveMeasureDetailDto>(mapper.ConfigurationProvider)
            //                        .GroupBy(x=>x.Code).Select(z=>z.FirstOrDefault())
            //                        .ToListAsync();
            return RequestResponse.Ok(new RiskAndPreventiveMeasuresListDropdownResponse(chapter, subChapterCurrent, activityCurrent, risk, measure, subChapterIdList, subChapterId, activityId, borradorExist));
        }
    }
}

