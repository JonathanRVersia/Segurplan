using System;
using Microsoft.AspNetCore.Authorization;

namespace Segurplan.Web.Authorization {
    public class HasPermissionClaimRequirement : IAuthorizationRequirement {
        public string ClaimValue { get; }

        public HasPermissionClaimRequirement(string claimValue) {
            ClaimValue = claimValue ?? throw new ArgumentNullException(nameof(claimValue));
        }
    }
}
