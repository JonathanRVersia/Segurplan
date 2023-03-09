using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Segurplan.Core.Actions.Administration.ChapterDetails.Models;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Create {
    public class CreateChapterRequestHandler : IRequestHandler<CreateChapterRequest, IRequestResponse<CreateChapterResponse>> {
        private readonly SegurplanContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;

        public CreateChapterRequestHandler(SegurplanContext context, IMapper mapper, IHttpContextAccessor contextAccessor) {
            this.context = context;
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IRequestResponse<CreateChapterResponse>> Handle(CreateChapterRequest request, CancellationToken cancellationToken) {
            var chapterVersion = CreateChapter(request.Title);

            //context.Add(chapterVersion);

            //int changes = await context.SaveChangesAsync();

            return RequestResponse.Ok(new CreateChapterResponse(mapper.Map<ChapterDetailsChapterVersion>(chapterVersion)));
        }

        private ChapterVersion CreateChapter(string chapterTitle) {
            ChapterVersion chapterVersion = new ChapterVersion();
            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            chapterVersion.CreatedBy = userId;
            chapterVersion.ModifiedBy = userId;
            chapterVersion.CreateDate = DateTime.Now;
            chapterVersion.UpdateDate = DateTime.Now;
            chapterVersion.Title = chapterTitle;
            chapterVersion.Number = (context.ChapterVersion.Max(p => (int?)p.Number) ?? 0) + 1;
            chapterVersion.IdChapterNavigation = new Chapter {
                Number = 1,
                Title = chapterVersion.Title,
                CreatedBy = userId,
                ModifiedBy = userId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };

            return chapterVersion;
        }
    }
}
