using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Segurplan.FrameworkExtensions.Identity {
    [Serializable]
    public class IdentityResultException : Exception {
        public IdentityResultException(IdentityResult identityResult)
            : base(identityResult.Errors.Aggregate(string.Empty, (s, e) => s + $"\n{e.Code} - {e.Description}")) {

        }

        public IdentityResultException(IdentityResult identityResult, Exception inner)
            : base(identityResult.Errors.Aggregate(string.Empty, (s, e) => s + $"\n{e.Code} - {e.Description}"), inner) {
        }

        protected IdentityResultException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
