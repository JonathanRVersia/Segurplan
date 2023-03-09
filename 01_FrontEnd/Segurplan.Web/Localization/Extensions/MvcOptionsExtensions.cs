using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Segurplan.Web.Localization.Extensions {
    public static class MvcOptionsExtensions {
        public static void ConfigureModelBindingMessages(this IMvcBuilder mvcBuilder) {
            mvcBuilder?.Services.Configure<MvcOptions>(opt => {
                var stringLocalizerFactory = mvcBuilder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();

                var loc = stringLocalizerFactory.Create(typeof(SharedResource));

                opt.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(
                    prop => loc["Validation.MissingBindRequired", prop]);
                opt.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
                    () => loc["Validation.MissingKeyOrValue"]);
                opt.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(
                    () => loc["Validation.MissingRequestBodyRequired"]);
                opt.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    prop => loc["Validation.ValueMustNotBeNull"]);
                opt.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
                    (value, prop) => loc["Validation.AttemptedValueIsInvalid", value, prop]);
                opt.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(
                    value => loc["Validation.NonPropertyAttemptedValue", value]);
                opt.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(
                    prop => loc["Validation.UnknownValueIsInvalid", prop]);
                opt.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(
                    () => loc["Validation.NonPropertyUnknownValueIsInvalid"]);
                opt.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
                    value => loc["Validation.ValueIsInvalid", value]);
                opt.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
                    prop => loc["Validation.ValueMustBeANumber", prop]);
                opt.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(
                    () => loc["Validation.NonPropertyValueMustBeNumber"]);
            });
        }
    }
}
