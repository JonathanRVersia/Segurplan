using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Segurplan.Web.Authorization {
    public class ApplicationClaimsPolicyProvider : DefaultAuthorizationPolicyProvider {
        private readonly AuthorizationOptions options;

        public ApplicationClaimsPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) {
            this.options = options?.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName) {
            var policy = await base.GetPolicyAsync(policyName).ConfigureAwait(true);

            if (policy == null) {
                policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new HasPermissionClaimRequirement(policyName))
                    .Build();

                options.AddPolicy(policyName, policy);
            }

            return policy;
        }
    }
}
