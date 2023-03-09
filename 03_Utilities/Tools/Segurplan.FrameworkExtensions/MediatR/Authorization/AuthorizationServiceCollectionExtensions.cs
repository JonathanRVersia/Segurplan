using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Segurplan.FrameworkExtensions.MediatR.Authorization {
    public static class AuthorizationServiceCollectionExtensions {
        public static IServiceCollection AddMediatRAuthorization(this IServiceCollection services) {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));

            return services;
        }
    }
}
