using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Segurplan.FrameworkExtensions.Identity;

namespace Segurplan.Web.Authorization {
    public class HasPermissionClaimHandler : AuthorizationHandler<HasPermissionClaimRequirement> {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasPermissionClaimRequirement requirement) {
            if (context.User.HasClaim(c => c.Type == SegurplanClaimTypes.Permission && c.Value == requirement.ClaimValue))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
