using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.Reflection;
using Segurplan.Infrastructure.EntityFramework.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using OldDBDataMigrator.ProduccionDBModels;
using Microsoft.AspNetCore.Authentication;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR.Validation;
using Segurplan.FrameworkExtensions.MediatR.Authorization;
using AutoMapper;
using Segurplan.Core;
using System.Reflection;
using OldDBDataMigrator.Utils;
using OldDBDataMigrator.DataMigration.ConsoleSeeds;
using Segurplan.Core.Helpers.ActiveDirectory;
using OldDBDataMigrator.DataMigration.Templates;
using OldDBDataMigrator.DataMigration.CleanTexts;
using OldDBDataMigrator.DataMigration.CleanProducedBy;
using OldDBDataMigrator.DataMigration.PreventiveMeasuresOrderUpdate;
using OldDBDataMigrator.DataMigration.DuplicateRiskAssignments;

namespace OldDBDataMigrator {
    class Program {
        private static Assembly ThisAssembly => typeof(Program).Assembly;

        static void Main(string[] args) => CreateHostBuilder(args).Build().RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             new HostBuilder()
                .UseEnvironment("Development")
                .ConfigureAppConfiguration((ctx, cfg) => cfg
                .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true))
                .ConfigureLogging((ctx, l) => {
                    l.AddConfiguration(ctx.Configuration);
                    l.AddConsole();
                })
                .ConfigureServices((hostContext, services) => {

                    var dataMigratorServicesOptions = new SegurplanServicesOptions()
                                           .AddMapperProfilesAssemblies(ThisAssembly)
                                           .AddValidatorsAssemblies(ThisAssembly);

                    services.AddOptions();
                    services.AddSegurplanSqlServer(hostContext.HostingEnvironment, hostContext.Configuration, migrationsAssembly: typeof(Program).Assembly.FullName);

                    services.AddIdentity<User, IdentityRole<int>>()
                            .AddRoles<IdentityRole<int>>()
                            .AddEntityFrameworkStores<SegurplanContext>();

                    services.AddDbContext<SegurplanProduccionContext>(opts => {

                        opts.UseSqlServer(hostContext.Configuration.GetConnectionString(nameof(SegurplanProduccionContext)), sql => {
                            sql.UseRelationalNulls()
                               .EnableRetryOnFailure();
                        })
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                        opts.EnableSensitiveDataLogging();
                    });

                    services.AddMediatR(typeof(Program).Assembly)
                    .AddMediatRValidation()
                    .AddMediatRAuthorization();

                    services.AddAutoMapper(dataMigratorServicesOptions.MapperProfilesAssemblies);

                    ////services.AddTransient<DatabaseMigrator>();
                    services.AddSingleton<DelegateFactory>();

                    //services.AddSingleton<GetFromFile>();
                    services.AddTransient<RunApplication>();

                    services.AddSingleton<ISystemClock, SystemClock>();

                    services.AddHostedService<Main>();

                    services.AddTransient<SeedUtils>();

                    services.AddTransient<ActiveDirectoryService>();

                    services.AddTransient<ActiveDirectoryOptions>();

                    services.AddTransient<UsersConsoleProgram>();

                    services.AddTransient<UpdateTemplates>();

                    services.AddTransient<UpdateAnagram>();

                    services.AddTransient<UpdatePreventiveMeasuresOrder>();

                    services.AddTransient<TextCleaner>();

                    services.AddTransient<ProducedByCleaner>();

                    services.AddTransient<DuplicateRiskAssignments>();

                    services.Scan(scan => scan.FromAssemblyOf<Program>()
                            .AddClasses(@class => @class.AssignableTo<ISeedInitializer>())
                            .As<ISeedInitializer>()
                            .WithTransientLifetime());

                });
    }
}
