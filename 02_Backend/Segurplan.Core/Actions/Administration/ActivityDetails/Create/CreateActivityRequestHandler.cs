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
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Create {
    public class CreateActivityRequestHandler : IRequestHandler<CreateActivityRequest, IRequestResponse<CreateActivityResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;

        public CreateActivityRequestHandler(SegurplanContext context, IMapper mapper, IHttpContextAccessor contextAccessor) {
            this.context = context;
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IRequestResponse<CreateActivityResponse>> Handle(CreateActivityRequest request, CancellationToken cancellationToken) {
            var activityVersion = CreateActivity(request);

            activityVersion.IdActivityNavigation.SubChapter = await context.SubChapter.Where(sch => sch.Id == request.SubChapterId).Include(x => x.IdChapterNavigation).FirstOrDefaultAsync();

            //context.Add(activityVersion);

            //int changes = await context.SaveChangesAsync();

            //var createdActivityVersion = await context.ActivityVersion.ProjectTo<ActivityDetailsActivityVersion>(mapper.ConfigurationProvider).Where(av => av.Id == activityVersion.Id).FirstOrDefaultAsync();

            return RequestResponse.Ok(new CreateActivityResponse(mapper.Map<ActivityDetailsActivityVersion>(activityVersion)));
        }

        private ActivityVersion CreateActivity(CreateActivityRequest request) {
            ActivityVersion activityVersion = new ActivityVersion();
            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            activityVersion.CreatedBy = userId;
            activityVersion.ModifiedBy = userId;
            activityVersion.CreateDate = DateTime.Now;
            activityVersion.UpdateDate = DateTime.Now;
            //subChapterVersion.IdVersionInfoNavigation = chapterVersionInfo;
            activityVersion.Description = request.Title;
            activityVersion.IdSubChapterVersion = request.SubChapterVersionId;
            activityVersion.Number = context.ActivityVersion.Max(p => p.Number) + 1;
            activityVersion.Number = (context.ActivityVersion.Where(p => p.IdSubChapterVersion == request.SubChapterVersionId).Max(p => (int?)p.Number) ?? 0) + 1;
            activityVersion.IdActivityNavigation = new Activity {
                Number = 1,
                Description = request.Title,
                CreatedBy = userId,
                ModifiedBy = userId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                SubChapterId = request.SubChapterId
            };

            return activityVersion;
        }
    }
}
