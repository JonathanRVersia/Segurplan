using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.GetLastChapterVersionId {
    public class GetLastChapterVersionIdRequestHandler : IRequestHandler<GetLastChapterVersionIdRequest, IRequestResponse<GetLastChapterVersionIdResponse>> {

        private readonly SegurplanContext context;

        public GetLastChapterVersionIdRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<GetLastChapterVersionIdResponse>> Handle(GetLastChapterVersionIdRequest request, CancellationToken cancellationToken) {
            //ch.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault()
            var chapterVersionId = await context.ChapterVersion.Where(chv => chv.IdChapter == request.ChapterId).OrderByDescending(x => x.VersionNumber).Select(x=>x.Id).FirstOrDefaultAsync();

            return RequestResponse.Ok(new GetLastChapterVersionIdResponse { ChapterVersionId = chapterVersionId });
        }
    }
}
