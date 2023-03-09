using System;
using System.Collections.Generic;
using System.DirectoryServices;
using Microsoft.Extensions.Options;
using Segurplan.Core.Domain.Identity;

namespace Segurplan.Infrastucture.Authentication.ActiveDirectory.ActiveDirectory {
 
    public class ActiveDirectoryAuthenticationProvider : IExternalAuthenticationProvider, IDisposable {
    
        private ActiveDirectoryAuthenticationOptions options;
        private readonly IDisposable optionsChangeSubscription;

        public string Name => ActiveDirectoryAuthenticationConstants.ProviderName;

        public ActiveDirectoryAuthenticationProvider(
            IOptionsMonitor<ActiveDirectoryAuthenticationOptions> options) {
            this.options = options.CurrentValue;
            optionsChangeSubscription = options.OnChange(o => this.options = o);
        }

        public string FormatExternalUserName(string domainName, string userName) {
            return $"{domainName}{ActiveDirectoryAuthenticationConstants.LogonSeparator}{userName}";
        }

        public Dictionary<string, string> AuthenticateUser(string userName, string password) {
            var properties = new Dictionary<string, string>();

            using (var searcher = new DirectorySearcher(new DirectoryEntry(options.ConnectionString) { Username = userName, Password = password }) {
                //Email or username filter loggin
                Filter = "(&(objectCategory=User)(objectClass=person)(sAMAccountName=" + userName + "))"
            }) {
                SearchResult result = null;
                try {
                    result = searcher.FindOne();
                } catch (Exception) {
                    // TODO: Log exception 
                }


                if (result == null)
                    return null;

                foreach (string property in result.Properties.PropertyNames) {
                    switch (property.ToUpper()) {
                        case "MAIL":
                            properties.Add(property, result.Properties[property][0].ToString());
                            break;
                        case "OBJECTGUID":
                            var objectGuid = (byte[])result.Properties[property][0];
                            properties.Add(property, new Guid(objectGuid).ToString());
                            break;
                    }
                }
                return properties;
            }
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    optionsChangeSubscription.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
    }
}
