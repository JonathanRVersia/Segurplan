using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Segurplan.Web.Localization.Extensions {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseSegurplanRequestLocalization(this IApplicationBuilder app) {
            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

            return app.UseRequestLocalization(localizationOptions.Value);
        }
    }
}
