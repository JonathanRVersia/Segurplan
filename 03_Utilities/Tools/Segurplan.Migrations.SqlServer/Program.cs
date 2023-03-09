using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Database;
using Segurplan.Core.Helpers.ActiveDirectory;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.Reflection;
using Segurplan.Infrastructure.EntityFramework.SqlServer;

namespace Segurplan.Migrations.SqlServer {
    internal class Program {
        private static void Main(string[] args) {
            CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return new HostBuilder()
                .UseEnvironment("Development")
                .ConfigureAppConfiguration((ctx, cfg) => cfg
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true))
                .ConfigureLogging((ctx, l) => {
                    l.AddConfiguration(ctx.Configuration);
                    l.AddConsole();
                })
                .ConfigureServices((hostContext, services) => {
                    services.AddOptions();
                    services.AddSegurplanSqlServer(hostContext.HostingEnvironment, hostContext.Configuration, migrationsAssembly: typeof(Program).Assembly.FullName);

                    services.AddIdentity<User, IdentityRole<int>>()
                            .AddRoles<IdentityRole<int>>()
                            .AddEntityFrameworkStores<SegurplanContext>();
                    services.Configure<ActiveDirectoryOptions>("Active.Directory", hostContext.Configuration);

                    services.AddSingleton<DelegateFactory>();
                    services.AddTransient<DatabaseMigrator>();
                    services.AddTransient<DatabaseInitializer>();
                    services.Scan(scan => scan.FromAssemblyOf<DatabaseInitializer>()
                            .AddClasses(@class => @class.AssignableTo<IDataSeed>())
                            .As<IDataSeed>()
                            .WithTransientLifetime());

                    services.AddHostedService<MigrationService>();
                    services.AddTransient<ActiveDirectoryService>();
                    services.AddTransient<ActiveDirectoryOptions>();
                });
        }
    }
}
