using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Segurplan.Core.Domain.Identity;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Identity.Login {

    public class LoginUserRequestHandler : IRequestHandler<LoginUserRequest, IRequestResponse<LoginUserResponse>> {

        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IExternalAuthenticationProvider externalAuthenticationProvider;
        private const string LoginProvider = "Active.Directory";

        public LoginUserRequestHandler(
            SignInManager<User> signInManager,
            IExternalAuthenticationProvider externalAuthenticationProvider,
            UserManager<User> userManager) {

            this.signInManager = signInManager;
            this.userManager = userManager;
            this.externalAuthenticationProvider = externalAuthenticationProvider;
        }

        public async Task<IRequestResponse<LoginUserResponse>> Handle(LoginUserRequest request, CancellationToken cancellationToken) {

            var signInResult = SignInResult.Failed;

            if (await userManager.FindByNameAsync(request.UserName) != null)
                signInResult = await AuthenticateUser(request, signInResult);

            return signInResult.Succeeded
                ? RequestResponse.Ok(new LoginUserResponse(signInResult))
                : RequestResponse.NotOk(new LoginUserResponse(signInResult));
        }

        private async Task<SignInResult> AuthenticateUser(LoginUserRequest request, SignInResult signInResult) {
            var ldapProperties = externalAuthenticationProvider.AuthenticateUser(request.UserName, request.Password);

            if (ldapProperties != null) {
                signInResult = SignInResult.Success;
            }

            return signInResult;
        }
    }
}
