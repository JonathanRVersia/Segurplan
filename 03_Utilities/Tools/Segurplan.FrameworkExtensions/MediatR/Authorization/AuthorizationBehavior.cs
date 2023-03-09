using MediatR;
using Microsoft.AspNetCore.Http;
using Segurplan.FrameworkExtensions.Identity;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.MediatR.Authorization;
using Segurplan.FrameworkExtensions.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Segurplan.FrameworkExtensions.MediatR.Authorization {
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : IRequestResponse {
        private readonly RequestAuthorizeAttribute requestAuthorizeAttribute;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly DelegateFactory delegateFactory;
        private Func<TRequest, IEnumerable<string>> getClaimsFromRequest;

        public AuthorizationBehavior(
            IHttpContextAccessor contextAccessor,
            DelegateFactory delegateFactory) {
            this.contextAccessor = contextAccessor;
            this.delegateFactory = delegateFactory;
            requestAuthorizeAttribute = typeof(TRequest).GetCustomAttribute<RequestAuthorizeAttribute>();

            if (string.IsNullOrEmpty(requestAuthorizeAttribute?.ClaimsMethod))
                return;

            getClaimsFromRequest = delegateFactory.CreateDelegate<Func<TRequest, IEnumerable<string>>>(new DelegateFactory.DelegateDescription() {
                MethodName = requestAuthorizeAttribute.ClaimsMethod,
                Reusable = true,
                TargetType = typeof(TRequest)
            });
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {
            if (requestAuthorizeAttribute == null)
                return next();

            var requiredClaims = requestAuthorizeAttribute.Claims?.ToList() ?? new List<(string, string)>();
            var requiredDynamicClaims = getClaimsFromRequest?.Invoke(request)
                                                   ?.Select(c => (SegurplanClaimTypes.Permission, c)) ?? Enumerable.Empty<(string, string)>();
            requiredClaims.AddRange(requiredDynamicClaims);

            if (!requiredClaims.Any())
                return next();

            var principal = contextAccessor.HttpContext.User;

            if (!requiredClaims.All(c => principal.HasClaim(c.type, c.value))) {
                return Task.FromResult(RequestResponse.OfType<TResponse>(delegateFactory, nameof(RequestResponse.Unauthorized)));
            }

            return next();
        }
    }
}
