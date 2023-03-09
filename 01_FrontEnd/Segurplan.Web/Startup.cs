using System;
using System.Reflection;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Segurplan.Core;
using Segurplan.Core.Domain.CacheServices;
using Segurplan.Core.Domain.Documents;
using Segurplan.DataAccessLayer;
using Segurplan.Infrastructure.EntityFramework.SqlServer;
using Segurplan.Infrastucture.Authentication.ActiveDirectory;
using Segurplan.Web.Localization;
using Segurplan.Web.Localization.Extensions;

namespace Segurplan.Web {
    public class Startup {
        private static Assembly ThisAssembly => typeof(Startup).Assembly;

        public Startup(IConfiguration configuration, IHostingEnvironment environment) {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddSegurplanCore(opt => opt
                            .AddMapperProfilesAssemblies(ThisAssembly)
                            .AddValidatorsAssemblies(ThisAssembly));

            services.AddSegurplanSqlServer(Environment.IsDevelopment(), Configuration);
            services.AddSegurplanDAL();

            services.AddSingleton<ISystemClock, SystemClock>(); //!!
            services.AddSingleton<DocumentProcessor>();

            services.ConfigureHangfire(Configuration);

            if (Environment.IsDevelopment()) {
                services.AddSingleton<Infrastucture.Authentication.ActiveDirectory.Fake.FakeExternalAuthenticationProvider>();
                services.AddSingleton<Core.Domain.Identity.IExternalAuthenticationProvider>(svc =>
                    svc.GetRequiredService<Infrastucture.Authentication.ActiveDirectory.Fake.FakeExternalAuthenticationProvider>());
            } else {
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddSegurplanActiveDirectory();
            }

            services.AddSegurplanLocalization();

            services.AddSession(opts => {
                opts.Cookie.IsEssential = true;
                opts.Cookie.HttpOnly = true;
                opts.Cookie.Name = "SegurPlanSessionCookie";
                opts.IdleTimeout = TimeSpan.FromMinutes(480);
            });
            services.AddMemoryCache();


            services.Configure<FormOptions>(options => {
                options.MultipartBodyLengthLimit = 100000000;
                options.ValueLengthLimit = 100000000;
                options.MultipartHeadersLengthLimit = 100000000;
                options.ValueCountLimit = 10000;
            });

            services.AddMvc(opt => {
                opt.ModelMetadataDetailsProviders.Add(new CustomValidationMetadataProvider());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddFluentValidation()
            .AddDataAnnotationsLocalization(options => {
                options.DataAnnotationLocalizerProvider = (type, factory) => {
                    return factory.Create(typeof(SharedResource));
                };
            })
            // Opcion para agregar mas de una ruta personalizada a las paginas de Razor, de normal se agregara la ruta en el documento
            // con @Page "ruta"
            //.AddRazorPagesOptions(options => {
            //    options.Conventions.AddPageRoute("/Models/Account/Login", "/Account/Login");
            //    options.Conventions.AddPageRoute("/Models/Account/Login", "/Login");
            //})
            .ConfigureModelBindingMessages();

            services.AddAuthorization();
            services.AddSegurplanAuthorizationPolicies();
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(opts => {
                opts.LoginPath = "/Models/Account/Login";
                opts.AccessDeniedPath = "/Models/Account/AccessDenied";
                opts.LogoutPath = "/Models/Account/Logout";
                opts.SlidingExpiration = true;
                opts.ExpireTimeSpan = TimeSpan.FromHours(1);
                opts.Cookie.Name = "SegurPlanIdentityCookie";
                //opts.Cookie.SecurePolicy = Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
            });

            services.Configure<ActiveDirectoryAuthenticationOptions>(Configuration.GetSection("ActiveDirectory"));
            services.Configure<CacheOptions>(nameof(ActivitiesCacheService), Configuration.GetSection("Cache:Activities"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            //Add log4net to loggers
            loggerFactory.AddLog4Net();

            app.UseSegurplanRequestLocalization();

       
            app.UseHttpsRedirection();
            //app.UseStatusCodePagesWithRedirects("~/login");
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions {
                Authorization = new[] { new AuthorizationFilter() },
                DashboardTitle = "Gestión de los Jobs"
            });
        }
    }
}
