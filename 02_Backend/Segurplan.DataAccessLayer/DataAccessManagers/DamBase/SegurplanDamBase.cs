using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MemoryCache;

namespace Segurplan.DataAccessLayer.DataAccessManagers.DamBase {
    public abstract class SegurplanDamBase {
        protected readonly SegurplanContext context;
        protected readonly MemoryCacheService cacheService;

        protected SegurplanDamBase(
            SegurplanContext context,
            MemoryCacheService cacheService
            ) {
            this.context = context;
            this.cacheService = cacheService;
        }

        protected SegurplanDamBase(SegurplanContext context) {
            this.context = context;
        }

        public void StartTransaction() {
            if (context.Database.CurrentTransaction == null) {
                context.Database.BeginTransaction();
            }
        }

        public void CommitTransaction() {
            if (context.Database.CurrentTransaction != null) {
                context.Database.CommitTransaction();
            }
        }

        public void RollbackTransaction() {
            if (context.Database.CurrentTransaction != null) {
                context.Database.RollbackTransaction();
            }
        }
    }
}
