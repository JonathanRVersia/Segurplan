using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Segurplan.FrameworkExtensions.EntityFramework.ValueGenerators;

namespace Segurplan.FrameworkExtensions.EntityFramework.Behaviors.Default {
    public class RunValueGeneratorsOnUpdateBehavior<T> : IBeforeSaveChangesBehavior<T>
        where T : DbContext {
        public int Order => 0;

        public Task BeforeSaveChanges(T context, CancellationToken cancellationToken = default) {
            var propertiesGeneratedOnUpdate = context.ChangeTracker.Entries()
                                                          .Where(e => e.State == EntityState.Modified)
                                                          .SelectMany(e => e.Properties.Where(p => p.Metadata.FindAnnotation(nameof(ValueGeneratorBehavior)) != null));

            foreach (var property in propertiesGeneratedOnUpdate) {
                var valueGeneratorBehavior = property.Metadata.FindAnnotation(nameof(ValueGeneratorBehavior));

                if (valueGeneratorBehavior == null)
                    continue;

                if ((ValueGeneratorBehavior)valueGeneratorBehavior.Value == ValueGeneratorBehavior.OnlyWhenNotSet &&
                    HasValueSet(property))
                    continue;

                var valueGeneratorFactory = property.Metadata.GetValueGeneratorFactory();
                var generator = valueGeneratorFactory?.Invoke(property.Metadata, property.EntityEntry.Metadata);

                if (generator == null)
                    continue;

                property.CurrentValue = generator.Next(property.EntityEntry);
            }

            return Task.CompletedTask;

            bool HasValueSet(PropertyEntry propertyEntry) {
                return propertyEntry.CurrentValue != null && !propertyEntry.CurrentValue.Equals(default(T));
            }
        }
    }
}
