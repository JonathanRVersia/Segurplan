using Microsoft.AspNetCore.Identity;

namespace Segurplan.Core.Actions.Identity.Login {
    public class LoginUserResponse {
        public LoginUserResponse(SignInResult signInResult) {
            SignInResult = signInResult;
        }

        public SignInResult SignInResult { get; }
    }
}
