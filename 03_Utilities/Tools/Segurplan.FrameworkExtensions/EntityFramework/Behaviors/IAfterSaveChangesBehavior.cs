using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Segurplan.FrameworkExtensions.EntityFramework.Behaviors {
    public interface IAfterSaveChangesBehavior<T>
        where T : DbContext {
        int Order { get; }

        Task<bool> AfterSaveChanges(T context, int changes, CancellationToken cancellationToken = default);
    }
}
