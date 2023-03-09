using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;

namespace Segurplan.Migrations.SqlServer {
    public class DatabaseMigrator {
        private readonly SegurplanContext context;

        public DatabaseMigrator(SegurplanContext context) {
            this.context = context;
        }

        public Task Migrate(CancellationToken cancellationToken) {
            return context.Database.MigrateAsync(cancellationToken);
        }
    }
}
