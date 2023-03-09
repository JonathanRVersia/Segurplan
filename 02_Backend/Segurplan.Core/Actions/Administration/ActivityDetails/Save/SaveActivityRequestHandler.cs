using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Save {
    public class SaveActivityRequestHandler : IRequestHandler<SaveActivityRequest, IRequestResponse<SaveActivityResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;

        public SaveActivityRequestHandler(SegurplanContext context, IMapper mapper, IHttpContextAccessor contextAccessor) {
            this.context = context;
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IRequestResponse<SaveActivityResponse>> Handle(SaveActivityRequest request, CancellationToken cancellationToken) {
            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var activityVersion = new ActivityVersion();

            if (request.Id != 0) {
                activityVersion = await context.ActivityVersion.FirstOrDefaultAsync(av => av.Id == request.Id);
            } else {
                activityVersion = CreateActivity(request, userId);
            }

            activityVersion = mapper.Map(request, activityVersion);

            activityVersion.ModifiedBy = userId;
            activityVersion.UpdateDate = DateTime.Now;

            context.Update(activityVersion);

            int changes = await context.SaveChangesAsync();

            return changes > 0 ? RequestResponse.Ok(new SaveActivityResponse(activityVersion.Id))
                               : RequestResponse.NotOk<SaveActivityResponse>();
        }

        private ActivityVersion CreateActivity(SaveActivityRequest request,int userId) {
            ActivityVersion activityVersion = new ActivityVersion();

            activityVersion.CreatedBy = userId;
            activityVersion.CreateDate = DateTime.Now;
            activityVersion.Description = request.Description;
            activityVersion.IdSubChapterVersion = request.IdSubChapterVersion;

            activityVersion.IdActivityNavigation = new Activity {
                Number = request.Number,
                Description = request.Description,
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
