using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Identity.Logout {
    public class LogoutUserRequestHandler : IRequestHandler<LogoutUserRequest, IRequestResponse<LogoutUserResponse>> {

        private readonly SignInManager<User> signInManager;

        public LogoutUserRequestHandler(SignInManager<User> signInManager) {
            this.signInManager = signInManager;
        }

        public async Task<IRequestResponse<LogoutUserResponse>> Handle(LogoutUserRequest request, CancellationToken cancellationToken) {
            await signInManager.SignOutAsync();
            return RequestResponse.Ok(new LogoutUserResponse());
        }
    }
}
