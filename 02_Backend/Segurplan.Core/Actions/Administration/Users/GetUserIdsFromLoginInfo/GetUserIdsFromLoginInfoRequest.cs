using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Users.GetUserIdsFromLoginInfo {
    public class GetUserIdsFromLoginInfoRequest : IRequest<IRequestResponse<GetUserIdsFromLoginInfoResponse>> {
    }
}
