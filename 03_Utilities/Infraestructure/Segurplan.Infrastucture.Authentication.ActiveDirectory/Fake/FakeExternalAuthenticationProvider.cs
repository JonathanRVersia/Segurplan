using System.Collections.Generic;
using Segurplan.Core.Domain.Identity;

namespace Segurplan.Infrastucture.Authentication.ActiveDirectory.Fake {
    public class FakeExternalAuthenticationProvider : IExternalAuthenticationProvider {
        private readonly Dictionary<string, string> users = new Dictionary<string, string>();

        public string Name => nameof(FakeExternalAuthenticationProvider);

        public string FormatExternalUserName(string domain, string userName) {
            return $"{userName}@{domain}";
        }

        public Dictionary<string, string> AuthenticateUser(string userName, string password) {

            return new Dictionary<string, string> {
                {"mail", "elecnor_dev@oesia.com"},
                { "objectguid", "08b0eee1-11d7-40e5-b020-20d9447cd0a1"}
            };
        }

        public void AddExternalUser(string providerKey, string password) {
            users.Add(providerKey, password);
        }
    }
}

