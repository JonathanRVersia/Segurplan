using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Segurplan.DataAccessLayer.Database.Audit;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.EntityFramework.Behaviors;
using Segurplan.FrameworkExtensions.EntityFramework.Configurations;

namespace Segurplan.Core.Database {
    public partial class SegurplanContext : IdentityDbContext<User, IdentityRole<int>, int> {
        private readonly IEnumerable<IBeforeSaveChangesBehavior<SegurplanContext>> beforeSaveChangesBehaviors;
        private readonly IEnumerable<IAfterSaveChangesBehavior<SegurplanContext>> afterSaveChangesBehaviors;
        private readonly EntityTypeConfigurationCollection entityTypeConfigurations;
        private readonly List<HistoryEntry> historyEntries;
        public SegurplanContext() { }
        public SegurplanContext(
            DbContextOptions<SegurplanContext> options,
            IEnumerable<IBeforeSaveChangesBehavior<SegurplanContext>> beforeSaveChangesBehaviors,
            IEnumerable<IAfterSaveChangesBehavior<SegurplanContext>> afterSaveChangesBehaviors,
            EntityTypeConfigurationCollection entityTypeConfigurations)
            : base(options) {
            historyEntries = new List<HistoryEntry>();

            this.beforeSaveChangesBehaviors = beforeSaveChangesBehaviors;
            this.afterSaveChangesBehaviors = afterSaveChangesBehaviors;
            this.entityTypeConfigurations = entityTypeConfigurations;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            foreach (var beforeChangesBehavior in beforeSaveChangesBehaviors.OrderBy(b => b.Order)) {
                await beforeChangesBehavior.BeforeSaveChanges(this, cancellationToken);
            }

            var changes = await base.SaveChangesAsync(cancellationToken);
            var saveChanges = false;

            foreach (var afterChangesBehavior in afterSaveChangesBehaviors.OrderBy(b => b.Order)) {
                saveChanges |= await afterChangesBehavior.AfterSaveChanges(this, changes, cancellationToken);
            }

            if (saveChanges)
                await SaveChangesAsync(cancellationToken);

            return changes;
        }

        public IEnumerable<HistoryEntry> HistoryEntries => historyEntries;

        public void AddHistoryEntries(IEnumerable<HistoryEntry> historyEntries) {
            this.historyEntries.AddRange(historyEntries);
        }

        public void ClearHistory() {
            historyEntries.Clear();
        }
    }
}
