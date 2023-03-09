using Microsoft.AspNetCore.Authorization;
using Segurplan.Web.Authorization;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddSegurplanAuthorizationPolicies(this IServiceCollection services) {
            services.AddSingleton<IAuthorizationHandler, HasPermissionClaimHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, ApplicationClaimsPolicyProvider>();

            return services;
        }
    }
}
