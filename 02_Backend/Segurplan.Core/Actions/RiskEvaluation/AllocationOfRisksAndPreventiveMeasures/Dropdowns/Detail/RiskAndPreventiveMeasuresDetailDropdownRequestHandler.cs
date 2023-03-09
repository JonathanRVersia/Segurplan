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

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.Detail {
    public class RiskAndPreventiveMeasuresDetailDropdownRequestHandler : IRequestHandler<RiskAndPreventiveMeasuresDetailDropdownRequest, IRequestResponse<RiskAndPreventiveMeasuresDetailDropdownResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public RiskAndPreventiveMeasuresDetailDropdownRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<RiskAndPreventiveMeasuresDetailDropdownResponse>> Handle(RiskAndPreventiveMeasuresDetailDropdownRequest request, CancellationToken cancellationToken) {

            var chapter = await context.Chapter
                                     .ProjectTo<ChapterDropdownDto>(mapper.ConfigurationProvider)
                                     .OrderBy(chap => chap.Number)
                                     .ToListAsync();

            var risk = await context.Risk
                                     .ProjectTo<RiskDropdownDto>(mapper.ConfigurationProvider)
                                     .OrderBy(r=>r.Code)
                                     .ToListAsync();

            var probability = await context.Probability
                                     .ProjectTo<ProbabilityDropownsDto>(mapper.ConfigurationProvider)
                                     .ToListAsync();

            var seriousness = await context.Seriousness
                                     .ProjectTo<SeriousnessDropdownDto>(mapper.ConfigurationProvider)
                                     .ToListAsync();

            var chapterVerison = new List<ChapterVersionDto>();
            if (request.IsEdit == true) {
                if(request.Vigente == true) {
                    chapterVerison = await context.ChapterVersion
                                      .Where(x => x.ApprovementDate < DateTime.Now && x.EndDate == null || x.EndDate > DateTime.Now)
                                      .ProjectTo<ChapterVersionDto>(mapper.ConfigurationProvider)
                                      .OrderBy(x => x.Number)
                                      .ToListAsync();
                } else {
                    chapterVerison = await context.ChapterVersion
                                      .Where(x => x.ApprovementDate == null || x.ApprovementDate > DateTime.Now)
                                      .ProjectTo<ChapterVersionDto>(mapper.ConfigurationProvider)
                                      .OrderBy(x => x.Number)
                                      .ToListAsync();
                }
                
            } else {
                chapterVerison = await context.ChapterVersion
                                     .Where(x=> x.ApprovementDate == null || x.ApprovementDate > DateTime.Now)
                                     .ProjectTo<ChapterVersionDto>(mapper.ConfigurationProvider)
                                     .OrderBy(chap => chap.Number)
                                     .ToListAsync();
            }

            return RequestResponse.Ok(new RiskAndPreventiveMeasuresDetailDropdownResponse(chapter,risk, probability, seriousness, chapterVerison));
        }
    }
}
