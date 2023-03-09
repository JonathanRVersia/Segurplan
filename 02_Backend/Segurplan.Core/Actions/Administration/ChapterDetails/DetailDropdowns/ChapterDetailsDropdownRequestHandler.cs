using System.Collections.Generic;
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

namespace Segurplan.Core.Actions.Administration.ChapterDetails.DetailDropdowns {
    public class ChapterDetailsDropdownRequestHandler : IRequestHandler<ChapterDetailsDropdownRequest, IRequestResponse<ChapterDetailsDropdownResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public ChapterDetailsDropdownRequestHandler(IMapper mapper, SegurplanContext context) {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<IRequestResponse<ChapterDetailsDropdownResponse>> Handle(ChapterDetailsDropdownRequest request, CancellationToken cancellationToken) {

            var users = new List<ChapterDetailsUserInfo>();

            var userIds = await context.UserRoles.Where(x => x.RoleId == 1).Select(x => x.UserId).ToListAsync();

            if(userIds.Any())
                users = await context.User.Where(x => userIds.Contains(x.Id)).ProjectTo<ChapterDetailsUserInfo>(mapper.ConfigurationProvider).ToListAsync();

            return users.Any() ? RequestResponse.Ok(new ChapterDetailsDropdownResponse(users))
                               : RequestResponse.NotFound<ChapterDetailsDropdownResponse>();
        }
    }
}
