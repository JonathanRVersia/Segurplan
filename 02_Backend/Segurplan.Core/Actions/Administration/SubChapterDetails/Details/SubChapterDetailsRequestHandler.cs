using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.SubChapterDetails.Models;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Details {
    public class SubChapterDetailsRequestHandler : IRequestHandler<SubChapterDetailsRequest, IRequestResponse<SubChapterDetailsResponse>> {
        private readonly IMapper mapper;
        private readonly SegurplanContext context;

        public SubChapterDetailsRequestHandler(IMapper mapper, SegurplanContext context) {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<IRequestResponse<SubChapterDetailsResponse>> Handle(SubChapterDetailsRequest request, CancellationToken cancellationToken) {
            var subChapterVersion = await context.SubChapterVersion.ProjectTo<SubChapterDetailsSubChapterVersion>(mapper.ConfigurationProvider).Where(ch => ch.Id == request.SubChapterversionId).FirstOrDefaultAsync();

            if (subChapterVersion == null)
                return RequestResponse.NotFound<SubChapterDetailsResponse>();

            subChapterVersion.ActivityVersion = subChapterVersion.ActivityVersion.OrderBy(x => x.Number).ToList();

            return RequestResponse.Ok(new SubChapterDetailsResponse(subChapterVersion));
        }
    }
}
