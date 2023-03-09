using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;
using static Segurplan.Core.Actions.Administration.ChapterActivityList.ChapterActivityListResponse;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.GetChapterVersions {
    public class GetChapterVersionsRequestHandler : IRequestHandler<GetChapterVersionsRequest, IRequestResponse<GetChapterVersionsResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public GetChapterVersionsRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<GetChapterVersionsResponse>> Handle(GetChapterVersionsRequest request, CancellationToken cancellationToken) {

            var chapterVersions = await context.ChapterVersion.ProjectTo<ChapterActivityListChapterVersion>(mapper.ConfigurationProvider).Where(cv => cv.IdChapter == request.ChapterId).ToListAsync();

            return chapterVersions.Any() ? RequestResponse.Ok(new GetChapterVersionsResponse(chapterVersions))
                                : RequestResponse.NotFound<GetChapterVersionsResponse>();
        }
    }
}
