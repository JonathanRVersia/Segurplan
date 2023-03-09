using FluentValidation.Results;

using System;
using System.Collections.Generic;

namespace Segurplan.FrameworkExtensions.MediatR.Validation {

    public class ValidationResultWithTypeInfo : ValidationResult {
     
        public ValidationResultWithTypeInfo(Type validatedType) {
            ValidatedType = validatedType;
        }

        public ValidationResultWithTypeInfo(IEnumerable<ValidationFailure> failures, Type validatedType)
            : base(failures) {
            ValidatedType = validatedType;
        }

        public Type ValidatedType { get; }
    }
}
