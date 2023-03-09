using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Segurplan.FrameworkExtensions.MediatR.Validation {
    public static class ValidationServiceCollectionExtensions {
        public static IServiceCollection AddMediatRValidation(this IServiceCollection services) {
            services.AddSingleton<ValidatorCollection>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
