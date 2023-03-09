using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.SubChapterDetails.Models;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Create {
    public class CreateSubChapterRequestHandler : IRequestHandler<CreateSubChapterRequest, IRequestResponse<CreateSubChapterResponse>> {
        private readonly IMapper mapper;
        private readonly SegurplanContext context;
        private readonly IHttpContextAccessor contextAccessor;

        public CreateSubChapterRequestHandler(IMapper mapper, SegurplanContext context, IHttpContextAccessor contextAccessor) {
            this.mapper = mapper;
            this.context = context;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IRequestResponse<CreateSubChapterResponse>> Handle(CreateSubChapterRequest request, CancellationToken cancellationToken) {
            var subChapterVersion = CreateSubChapter(request);

            //context.Add(subChapterVersion);

            //int changes = await context.SaveChangesAsync();

            //var createdSubChapterVersion = await context.SubChapterVersion.ProjectTo<SubChapterDetailsSubChapterVersion>(mapper.ConfigurationProvider).Where(ch => ch.Id == subChapterVersion.Id).FirstOrDefaultAsync();

            var chapterVersion = await context.ChapterVersion.Where(x => x.Id == request.ChapterVersionId).Include(chv => chv.IdChapterNavigation).FirstOrDefaultAsync();

            subChapterVersion.IdChapterVersionNavigation = chapterVersion;
            subChapterVersion.IdSubChapterNavigation.IdChapterNavigation = chapterVersion.IdChapterNavigation;


            return RequestResponse.Ok(new CreateSubChapterResponse(mapper.Map<SubChapterDetailsSubChapterVersion>(subChapterVersion)));
        }

        private SubChapterVersion CreateSubChapter(CreateSubChapterRequest request) {
            SubChapterVersion subChapterVersion = new SubChapterVersion();
            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            subChapterVersion.CreatedBy = userId;
            subChapterVersion.ModifiedBy = userId;
            subChapterVersion.CreateDate = DateTime.Now;
            subChapterVersion.UpdateDate = DateTime.Now;
            //subChapterVersion.IdVersionInfoNavigation = chapterVersionInfo;
            subChapterVersion.Title = request.Title;
            subChapterVersion.IdChapterVersion = request.ChapterVersionId;
            subChapterVersion.Number = (context.SubChapterVersion.Where(p => p.IdChapterVersion == request.ChapterVersionId).Max(p => (int?)p.Number) ?? 0) + 1;
            subChapterVersion.IdSubChapterNavigation = new SubChapter {
                Number = 1,
                Title = subChapterVersion.Title,
                //IdVersionInfoNavigation = chapterVersionInfo,
                CreatedBy = userId,
                ModifiedBy = userId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IdChapter = request.ChapterId
            };

            return subChapterVersion;
        }
    }
}
