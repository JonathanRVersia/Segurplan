using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Segurplan.Core.Database;

namespace OldDBDataMigrator {
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<SegurplanContext> {
        public SegurplanContext CreateDbContext(string[] args) {
            return Program.CreateHostBuilder(args)
                          .Build()
                          .Services
                          .GetRequiredService<SegurplanContext>();
        }
    }
}
