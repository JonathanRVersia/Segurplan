using System.Globalization;

namespace Segurplan.Web.Localization {
    public static class Cultures {
        public const string DefaultCulture = "es-ES";

        public static readonly CultureInfo[] SupportedCultures = new[]
        {
            new CultureInfo(DefaultCulture),
            new CultureInfo("en-GB")
        };
    }
}
