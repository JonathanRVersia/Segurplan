using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Segurplan.FrameworkExtensions.Identity {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddApplicationClaims<T>(this IServiceCollection services) {
            return AddApplicationClaims(services, typeof(T));
        }

        public static IServiceCollection AddApplicationClaims(this IServiceCollection services, Type fromType) {
            return AddApplicationClaims(services, fromType.Assembly);
        }

        public static IServiceCollection AddApplicationClaims(this IServiceCollection services, params Assembly[] assemblies) {
            services.Scan(scan => scan.FromAssemblies(assemblies)
                                      .AddClasses(@class => @class.AssignableTo<ClaimsCollection>())
                                      .As<ClaimsCollection>()
                                      .WithSingletonLifetime());

            services.AddSingleton<AvailableClaims>();

            return services;
        }
    }
}
