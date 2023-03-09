using System.Collections.Generic;

namespace Segurplan.Core.Domain.Identity {
    
    public interface IExternalAuthenticationProvider {
    
        string Name { get; }

        string FormatExternalUserName(string domainName, string userName);

        Dictionary<string, string> AuthenticateUser(string userName, string password);
    }
}
