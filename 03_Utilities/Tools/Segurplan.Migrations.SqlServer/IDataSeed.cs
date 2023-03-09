using System.Threading;
using System.Threading.Tasks;
using Segurplan.Core.Database;

namespace Segurplan.Migrations.SqlServer {
    public interface IDataSeed {
        Task Seed(SegurplanContext context, CancellationToken cancellationToken = default);
    }
}
