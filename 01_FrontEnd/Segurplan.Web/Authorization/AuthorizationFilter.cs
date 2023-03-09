using Hangfire.Dashboard;

namespace Microsoft.Extensions.DependencyInjection {
    public class AuthorizationFilter : IDashboardAuthorizationFilter {
        public bool Authorize(DashboardContext context) {
            var httpContext = context.GetHttpContext();
            return httpContext.User.HasClaim(x => x.Value == "Administrador");
        }
    }
}
