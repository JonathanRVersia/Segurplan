using Microsoft.Extensions.DependencyInjection;

namespace Segurplan.FrameworkExtensions.Reflection {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddDelegateFactory(this IServiceCollection services) {
            services.AddSingleton<DelegateFactory>();

            return services;
        }
    }
}
