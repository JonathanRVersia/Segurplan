using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;


namespace Segurplan.FrameworkExtensions.MediatR.Validation {
    
    public abstract class RequestValidator<T> : AbstractValidator<T> {
    
        public RequestValidator() {
        }

        public override ValidationResult Validate(ValidationContext<T> context) {
            var result = base.Validate(context);

            return result.For<T>();
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellation = default) {
            var result = await base.ValidateAsync(context, cancellation);

            return result.For<T>();
        }
    }
}
