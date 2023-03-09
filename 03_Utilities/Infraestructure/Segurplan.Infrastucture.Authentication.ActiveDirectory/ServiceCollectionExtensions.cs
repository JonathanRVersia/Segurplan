using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Segurplan.Core.Domain.Identity;
using Segurplan.Infrastucture.Authentication.ActiveDirectory.ActiveDirectory;


namespace Segurplan.Infrastucture.Authentication.ActiveDirectory {
   
    public static class ServiceCollectionExtensions {
    
        public static AuthenticationBuilder AddSegurplanActiveDirectory(this AuthenticationBuilder builder) {
        
            builder.Services.AddSingleton<IExternalAuthenticationProvider, ActiveDirectoryAuthenticationProvider>();

            return builder;
        }

        public static AuthenticationBuilder AddSegurplanActiveDirectory(this AuthenticationBuilder builder, Action<ActiveDirectoryAuthenticationOptions> configureOptions) {
        
            AddSegurplanActiveDirectory(builder);

            builder.Services.Configure(configureOptions);

            return builder;
        }
    }
}
