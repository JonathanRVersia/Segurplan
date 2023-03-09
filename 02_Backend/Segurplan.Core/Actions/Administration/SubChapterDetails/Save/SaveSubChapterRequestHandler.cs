using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.ActivityDetails.Delete;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Save {
    public class SaveSubChapterRequestHandler : IRequestHandler<SaveSubChapterRequest, IRequestResponse<SaveSubChapterResponse>> {
        private readonly IMapper mapper;
        private readonly SegurplanContext context;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IMediator mediator;

        public SaveSubChapterRequestHandler(IMapper mapper, SegurplanContext context, IHttpContextAccessor contextAccessor, IMediator mediator) {
            this.mapper = mapper;
            this.context = context;
            this.contextAccessor = contextAccessor;
            this.mediator = mediator;
        }

        public async Task<IRequestResponse<SaveSubChapterResponse>> Handle(SaveSubChapterRequest request, CancellationToken cancellationToken) {

            var subChapterVersion = new SubChapterVersion();
            int userId = int.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (request.Id != 0) {
                if (!string.IsNullOrEmpty(request.RemoveActivitiesIds)) {
                    var activityIds = request.RemoveActivitiesIds.Split(",").Select(Int32.Parse).ToList();
                    foreach (var activityId in activityIds) {
                        await mediator.Send(new DeleteActivityRequest { ActivityId = activityId });
                    }
                }

                subChapterVersion = await context.SubChapterVersion.FirstOrDefaultAsync(ch => ch.Id == request.Id);
            } else {
                subChapterVersion = CreateSubChapter(request, userId);
            }


            subChapterVersion = mapper.Map(request, subChapterVersion);
            subChapterVersion.ModifiedBy = userId;
            subChapterVersion.UpdateDate = DateTime.Now;

            context.Update(subChapterVersion);

            int changes = await context.SaveChangesAsync();

            return changes > 0 ? RequestResponse.Ok(new SaveSubChapterResponse(subChapterVersion.Id, subChapterVersion.IdSubChapter))
                               : RequestResponse.NotOk<SaveSubChapterResponse>();
        }

        private SubChapterVersion CreateSubChapter(SaveSubChapterRequest request, int userId) {
            SubChapterVersion subChapterVersion = new SubChapterVersion();

            subChapterVersion.CreatedBy = userId;
            subChapterVersion.ModifiedBy = userId;
            subChapterVersion.CreateDate = DateTime.Now;
            subChapterVersion.Title = request.Title;
            subChapterVersion.IdChapterVersion = request.IdChapterVersion;

            subChapterVersion.IdSubChapterNavigation = new SubChapter {
                Number = request.Number,
                Title = subChapterVersion.Title,
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
