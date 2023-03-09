using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.ChapterDetails.Models;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Details {
    public class ChapterDetailsRequestHandler : IRequestHandler<ChapterDetailsRequest, IRequestResponse<ChapterDetailsResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public ChapterDetailsRequestHandler(SegurplanContext context,IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<ChapterDetailsResponse>> Handle(ChapterDetailsRequest request, CancellationToken cancellationToken) {

            var chapterVersion = await context.ChapterVersion.ProjectTo<ChapterDetailsChapterVersion>(mapper.ConfigurationProvider).Where(ch => ch.Id == request.ChapterversionId).FirstOrDefaultAsync();

            if (chapterVersion == null)
                return RequestResponse.NotFound<ChapterDetailsResponse>();

            chapterVersion.SubChapterVersion= chapterVersion.SubChapterVersion.OrderBy(x => x.Number).ToList();

            return RequestResponse.Ok(new ChapterDetailsResponse(chapterVersion));
        }

    }
}
