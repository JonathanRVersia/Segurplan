
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Segurplan.Core.Database;
using AspNetCoreHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using CommonHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Segurplan.Infrastructure.EntityFramework.SqlServer {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddSegurplanSqlServer(this IServiceCollection services, CommonHostingEnvironment env, IConfiguration config, string migrationsAssembly = null) {
            return AddSegurplanSqlServer(services, env.IsDevelopment(), config, migrationsAssembly);
        }

        public static IServiceCollection AddSegurplanSqlServer(this IServiceCollection services, AspNetCoreHostingEnvironment env, IConfiguration config, string migrationsAssembly = null) {
            return AddSegurplanSqlServer(services, env.IsDevelopment(), config, migrationsAssembly);
        }

        public static IServiceCollection AddSegurplanSqlServer(this IServiceCollection services, bool isDevelopment, IConfiguration config, string migrationsAssembly = null) {
            services.AddDbContext<SegurplanContext>(opts => {
#if DEBUG
                if (migrationsAssembly == null) {
                    opts.UseLoggerFactory(
                        new Microsoft.Extensions.Logging.LoggerFactory(new[] {
                        new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
                        })
                    );
                }
#endif
                opts.UseSqlServer(config.GetConnectionString(nameof(SegurplanContext)), sql => {
                    sql.UseRelationalNulls()
                       .EnableRetryOnFailure();

                    if (!string.IsNullOrEmpty(migrationsAssembly))
                        sql.MigrationsAssembly(migrationsAssembly);
                })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                if (isDevelopment)
                    opts.EnableSensitiveDataLogging();
            })
            .AddEntityFrameworkBehaviors<SegurplanContext>(typeof(SegurplanContext).Assembly, typeof(ServiceCollectionExtensions).Assembly)
            .AddEntityTypeConfigurations(typeof(SegurplanContext).Assembly, typeof(ServiceCollectionExtensions).Assembly);

            return services;
        }
    }
}
