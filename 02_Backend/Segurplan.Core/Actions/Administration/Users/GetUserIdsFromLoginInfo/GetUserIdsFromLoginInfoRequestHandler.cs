using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Users.GetUserIdsFromLoginInfo {
    public class GetUserIdsFromLoginInfoRequestHandler : IRequestHandler<GetUserIdsFromLoginInfoRequest, IRequestResponse<GetUserIdsFromLoginInfoResponse>> {

        private readonly SegurplanContext context;

        public GetUserIdsFromLoginInfoRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<GetUserIdsFromLoginInfoResponse>> Handle(GetUserIdsFromLoginInfoRequest request, CancellationToken cancellationToken) {

            var userIds = await context.UserLogins.Select(x => x.UserId).ToListAsync();

            return userIds.Any() ? RequestResponse.Ok(new GetUserIdsFromLoginInfoResponse { UserIds = userIds })
                : RequestResponse.NotFound<GetUserIdsFromLoginInfoResponse>();
        }
    }
}
