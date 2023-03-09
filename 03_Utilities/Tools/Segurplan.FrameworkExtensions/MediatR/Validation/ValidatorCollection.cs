using System;
using FluentValidation;

namespace Segurplan.FrameworkExtensions.MediatR.Validation {
    public class ValidatorCollection {
        private readonly IServiceProvider serviceProvider;

        public ValidatorCollection(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }

        public IValidator GetValidatorFor(Type type) {
            var requestedValidator = typeof(IValidator<>).MakeGenericType(type);

            return serviceProvider.GetService(requestedValidator) as IValidator;
        }

    }
}
