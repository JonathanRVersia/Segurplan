using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Segurplan.Core.Helpers.CustomResetPasswordTokenProvider {
    public class CustomResetPasswordTokenProvider<TUser>
        : DataProtectorTokenProvider<TUser> where TUser : class {
        public CustomResetPasswordTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<CustomResetPasswordTokenProviderOptions> options)
            : base(dataProtectionProvider, options) {

        }
    }
}
