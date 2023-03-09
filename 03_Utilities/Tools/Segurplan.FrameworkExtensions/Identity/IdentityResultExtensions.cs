using Microsoft.AspNetCore.Identity;

namespace Segurplan.FrameworkExtensions.Identity {
    public static class IdentityResultExtensions {
        public static void EnsureSuccess(this IdentityResult result) {
            if (result.Succeeded)
                return;

            throw new IdentityResultException(result);
        }
    }
}
