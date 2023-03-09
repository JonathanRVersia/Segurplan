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
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterActivityList {
    public class ChapterActivityListRequestHandler: IRequestHandler<ChapterActivityListRequest, IRequestResponse<ChapterActivityListResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public ChapterActivityListRequestHandler(SegurplanContext context,IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public Task<IRequestResponse<ChapterActivityListResponse>> Handle(ChapterActivityListRequest request, CancellationToken cancellationToken) {

            var chapters = context.Chapter.ProjectTo<ChapterActivityListResponse.ListItem>(mapper.ConfigurationProvider).RunSpecificationSync(request.Specifications);

            if (!chapters.Results.Any())
                return Task.FromResult(RequestResponse.NotFound<ChapterActivityListResponse>());

            return Task.FromResult(RequestResponse.Ok(new ChapterActivityListResponse(chapters)));
        }
    }
}
    

