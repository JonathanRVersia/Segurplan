using Microsoft.Extensions.Configuration;

namespace Segurplan.Core.Helpers.ActiveDirectory {
    public class ActiveDirectoryOptions {
        public ActiveDirectoryOptions(IConfiguration configuration) {

            var config = configuration.GetSection("ActiveDirectory");

            UserName = config.GetValue<string>("UserName");
            UserPassword = config.GetValue<string>("UserPassword");
            ConnectionString = config.GetValue<string>("ConnectionString");
            ActiveDirectoryName = config.GetValue<string>("ActiveDirectoryName");
            ActiveDirectoryFilter = config.GetValue<string>("ActiveDirectoryFilter");
            LoginProvider = config.GetValue<string>("LoginProvider");
        }

        public string UserName { get; }
        public string UserPassword { get; }
        public string ConnectionString { get; }
        public string ActiveDirectoryName { get; }
        public string ActiveDirectoryFilter { get; }
        public string LoginProvider { get; }
    }
}
