using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Segurplan.FrameworkExtensions.EntityFramework.Configurations;

namespace Microsoft.EntityFrameworkCore {
    public static class ConfigurationsServiceCollectionExtensions {
        public static IServiceCollection AddEntityTypeConfigurations(this IServiceCollection services, params Assembly[] assemblies) {
            var configurationTypes = assemblies.SelectMany(a => a.DefinedTypes.Where(t => t.ImplementedInterfaces.Any(i => i.Name.Contains("IEntityTypeConfiguration"))));

            services.AddSingleton(svc => new EntityTypeConfigurationCollection(svc, configurationTypes));

            return services;
        }
    }
}
