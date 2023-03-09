using System;
using System.Collections.Generic;
using System.Linq;
using Segurplan.FrameworkExtensions.Identity;

namespace Segurplan.FrameworkExtensions.MediatR.Authorization {
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RequestAuthorizeAttribute : Attribute {
        public RequestAuthorizeAttribute(params string[] claims) {
            Claims = claims?.Select(c => (SegurplanClaimTypes.Permission, c)) ?? Enumerable.Empty<(string, string)>();
        }

        public IEnumerable<(string type, string value)> Claims { get; }

        public string ClaimsMethod { get; set; }
    }
}
