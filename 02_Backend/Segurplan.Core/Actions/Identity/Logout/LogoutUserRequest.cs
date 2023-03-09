using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Identity.Logout {
    public class LogoutUserRequest : IRequest<IRequestResponse<LogoutUserResponse>> {

    }
}
