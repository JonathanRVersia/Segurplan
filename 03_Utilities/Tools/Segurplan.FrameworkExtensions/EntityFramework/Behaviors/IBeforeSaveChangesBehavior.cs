using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Segurplan.FrameworkExtensions.EntityFramework.Behaviors {
    public interface IBeforeSaveChangesBehavior<T>
        where T : DbContext {
        int Order { get; }

        Task BeforeSaveChanges(T context, CancellationToken cancellationToken = default);
    }
}
