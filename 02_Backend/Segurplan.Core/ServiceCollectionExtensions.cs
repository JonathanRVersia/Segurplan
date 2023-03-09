using System;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Segurplan.Core.Database;
using Segurplan.Core.Domain.CacheServices;
using Segurplan.Core.Domain.Documents;
using Segurplan.Core.Helpers.ActiveDirectory;
using Segurplan.Core.Helpers.CustomResetPasswordTokenProvider;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.Identity;
using Segurplan.FrameworkExtensions.MediatR.Authorization;
using Segurplan.FrameworkExtensions.MediatR.Validation;
using Segurplan.FrameworkExtensions.MemoryCache;
using Segurplan.FrameworkExtensions.Reflection;

namespace Segurplan.Core {
    public static class ServiceCollectionExtensions {
        private static Assembly ThisAssembly => typeof(ServiceCollectionExtensions).Assembly;

        public static IServiceCollection AddSegurplanCore(this IServiceCollection services, Action<SegurplanServicesOptions> configure = null) {
            var coreServicesOptions = new SegurplanServicesOptions()
                                            .AddMapperProfilesAssemblies(ThisAssembly)
                                            .AddValidatorsAssemblies(ThisAssembly);

            configure?.Invoke(coreServicesOptions);

            services.AddOptions();
            services.AddMediatR(ThisAssembly)
                    .AddMediatRValidation()
                    .AddMediatRAuthorization()
                    .AddMemoryCacheTools();

            services.AddAutoMapper(coreServicesOptions.MapperProfilesAssemblies);
            services.AddValidatorsFromAssemblies(coreServicesOptions.ValidatorsAssemblies);
            //services.AddAutoMapper(ThisAssembly);
            //services.AddValidatorsFromAssembly(ThisAssembly);
            services.AddIdentity<User, IdentityRole<int>>(options => {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;

                options.Tokens.PasswordResetTokenProvider = "CustomResetPassword";
            }).AddDefaultTokenProviders()
            .AddTokenProvider<CustomResetPasswordTokenProvider<User>>("CustomResetPassword")
                    .AddSignInManager<SignInManager<User>>()
                    .AddRoles<IdentityRole<int>>()
                    .AddEntityFrameworkStores<SegurplanContext>();

            services.Configure<CustomResetPasswordTokenProviderOptions>(options => {
                options.Name = "CustomResetPassword";
                options.TokenLifespan = TimeSpan.FromHours(5);
            });

            services.AddDelegateFactory();
            services.AddApplicationClaims(ThisAssembly);
            //services.AddFlowManagementServices(ThisAssembly);
            //services.AddSingleton<ISystemClock, SystemClock>();

            services.AddTransient<ActivitiesCacheService>();
            services.AddTransient<IHtmlStringCleaner, HtmlStringCleaner>();

            services.AddTransient<ActiveDirectoryService>();
            services.AddTransient<ActiveDirectoryOptions>();

            return services;
        }
    }
}
