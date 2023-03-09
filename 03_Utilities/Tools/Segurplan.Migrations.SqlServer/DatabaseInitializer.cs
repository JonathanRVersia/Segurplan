using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Segurplan.Core.Database;

namespace Segurplan.Migrations.SqlServer {
    public class DatabaseInitializer {
        private readonly IEnumerable<IDataSeed> dataSeeds;
        private readonly SegurplanContext context;

        public DatabaseInitializer(
            IEnumerable<IDataSeed> dataSeeds,
            SegurplanContext context) {
            this.dataSeeds = dataSeeds;
            this.context = context;
        }

        public async Task Initialize(CancellationToken cancellationToken = default) {
            foreach (var seed in dataSeeds) {
                await seed.Seed(context, cancellationToken);
            }
        }
    }
}
