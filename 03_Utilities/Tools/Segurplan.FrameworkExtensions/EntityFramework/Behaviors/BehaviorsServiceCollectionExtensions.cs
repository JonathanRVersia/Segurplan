using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Segurplan.FrameworkExtensions.EntityFramework.Behaviors;
using Segurplan.FrameworkExtensions.EntityFramework.Behaviors.Default;

namespace Microsoft.EntityFrameworkCore {
    public static class BehaviorsServiceCollectionExtensions {
        public static IServiceCollection AddEntityFrameworkBehaviors<TContext>(this IServiceCollection services, params Assembly[] assemblies)
            where TContext : DbContext {
            assemblies = assemblies ?? Enumerable.Empty<Assembly>().ToArray();

            services.AddTransient<IBeforeSaveChangesBehavior<TContext>, RunValueGeneratorsOnUpdateBehavior<TContext>>();

            services.Scan(scan => scan
                                    .FromAssemblies(assemblies)
                                    .AddClasses(@class => @class.AssignableTo(typeof(IBeforeSaveChangesBehavior<TContext>)))
                                    .AddClasses(@class => @class.AssignableTo(typeof(IAfterSaveChangesBehavior<TContext>)))
                                    .AsImplementedInterfaces()
                                    .WithTransientLifetime());

            return services;
        }
    }
}
