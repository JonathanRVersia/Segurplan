using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace Segurplan.Web.Localization {
    public class SharedLocalizer : IHtmlLocalizer, IStringLocalizer {
        private readonly IHtmlLocalizer<SharedResource> localizer;

        public SharedLocalizer(IHtmlLocalizer<SharedResource> htmlLocalizer) {
            localizer = htmlLocalizer;
        }

        public LocalizedHtmlString this[string name] => localizer[name];

        public LocalizedHtmlString this[string name, params object[] arguments] => localizer[name, arguments];

        LocalizedString IStringLocalizer.this[string name] => localizer.GetString(name);

        LocalizedString IStringLocalizer.this[string name, params object[] arguments] => localizer.GetString(name, arguments);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) {
            return localizer.GetAllStrings(includeParentCultures);
        }

        public LocalizedString GetString(string name) {
            return localizer.GetString(name);
        }

        public LocalizedString GetString(string name, params object[] arguments) {
            return localizer.GetString(name, arguments);
        }

        public IHtmlLocalizer WithCulture(CultureInfo culture) {
            return localizer.WithCulture(culture);
        }

        IStringLocalizer IStringLocalizer.WithCulture(CultureInfo culture) {
            localizer.WithCulture(culture);

            return this;
        }
    }
}
