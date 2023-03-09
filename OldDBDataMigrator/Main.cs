using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OldDBDataMigrator {

    public class Main : IHostedService {
        private readonly IServiceProvider serviceProvider;

        public Main(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            using (var scope = serviceProvider.CreateScope()) {
                //var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
                var runApplication = scope.ServiceProvider.GetRequiredService<RunApplication>();

                //await migrator.Migrate(cancellationToken);
                await runApplication.Run(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}
