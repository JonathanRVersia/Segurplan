using MediatR;
using Segurplan.FrameworkExtensions.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Segurplan.FrameworkExtensions.MediatR.Validation {
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : IRequestResponse {
        private readonly ValidatorCollection validators;
        private readonly DelegateFactory delegateFactory;

        public ValidationBehavior(
            ValidatorCollection validators,
            DelegateFactory delegateFactory) {
            this.validators = validators;
            this.delegateFactory = delegateFactory;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next) {
            var validator = validators.GetValidatorFor(typeof(TRequest));

            if (validator == null)
                return await next();

            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return RequestResponse.OfType<TResponse>(delegateFactory, nameof(RequestResponse.NotOk), validationResult);

            var response = await next();
            response.ValidationResult = validationResult;

            return response;
        }
    }
}
