using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Segurplan.Web.Localization {
    public static class LocalizationServiceCollectionExtensions {
        public static void AddSegurplanLocalization(this IServiceCollection services, Action<RequestLocalizationOptions> configure = null) {
            services.AddLocalization();
            services.AddSingleton<SharedLocalizer>();
            services.AddSingleton<IStringLocalizer>(svc => svc.GetService<SharedLocalizer>());
            services.AddSingleton<IHtmlLocalizer>(svc => svc.GetService<SharedLocalizer>());

            services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture(Cultures.DefaultCulture);
                options.SupportedCultures = Cultures.SupportedCultures;
                options.SupportedUICultures = Cultures.SupportedCultures;
                options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
            });

            if (configure != null)
                services.Configure(configure);
        }
    }
}
