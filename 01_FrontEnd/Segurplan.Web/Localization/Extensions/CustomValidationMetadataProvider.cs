using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Segurplan.Web.Localization.Extensions {
    public class CustomValidationMetadataProvider : IValidationMetadataProvider {
        private const string ValidationKeyPrefix = "Validation.";

        // Original validation messages:
        // https://github.com/dotnet/corefx/blob/master/src/System.ComponentModel.Annotations/src/Resources/Strings.resx

        public void CreateValidationMetadata(ValidationMetadataProviderContext context) {
            if (context.Key.ModelType.GetTypeInfo().IsValueType
                && Nullable.GetUnderlyingType(context.Key.ModelType.GetType()) != null
                && !context.ValidationMetadata.ValidatorMetadata.OfType<RequiredAttribute>().Any()) {
                context.ValidationMetadata.ValidatorMetadata.Add(new RequiredAttribute());
            }

            foreach (var validationAttribute in context.ValidationMetadata.ValidatorMetadata.OfType<ValidationAttribute>()) {
                if (validationAttribute.ErrorMessageResourceName == null && validationAttribute.ErrorMessageResourceType == null) {
                    // By convention, the resource key will coincide with the attribute
                    // name, removing the suffix "Attribute" when needed
                    var resourceKey = validationAttribute.GetType().Name;
                    if (resourceKey.EndsWith("Attribute")) {
                        resourceKey = resourceKey.Substring(0, resourceKey.Length - 9);
                    }

                    // Patch the "StringLength with minimum value" case
                    if (validationAttribute is StringLengthAttribute stringLength && stringLength.MinimumLength > 0) {
                        resourceKey = "StringLengthIncludingMinimum";
                    }

                    resourceKey = ValidationKeyPrefix + resourceKey;

                    validationAttribute.ErrorMessage = resourceKey;
                    validationAttribute.ErrorMessageResourceType = null;
                    validationAttribute.ErrorMessageResourceName = null;
                }
            }
        }
    }
}
