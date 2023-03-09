using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Segurplan.Core.Domain.Audit {
    public abstract class HistoryEntry : IAuditable {
        private const string PrimaryKeyName = "Id";
        private readonly EntityEntry<ITracksHistory> entry;

        protected HistoryEntry() {

        }

        protected HistoryEntry(EntityEntry<ITracksHistory> entry) {
            var oldValues = new Dictionary<string, object>();
            var newValues = new Dictionary<string, object>();

            foreach (var property in entry.Properties) {
                var propertyName = property.Metadata.Name;

                if (property.Metadata.IsForeignKey()) {
                    continue;
                }

                if (property.Metadata.Name == PrimaryKeyName) {
                    AuditedEntityId = (int)property.CurrentValue;
                    continue;
                }

                switch (entry.State) {
                    case EntityState.Deleted:
                        oldValues.Add(propertyName, property.OriginalValue);
                        break;
                    case EntityState.Modified:
                        if (property.IsModified) {
                            oldValues.Add(propertyName, property.OriginalValue);
                            newValues.Add(propertyName, property.CurrentValue);
                        }
                        break;
                    case EntityState.Added:
                        newValues.Add(propertyName, property.CurrentValue);
                        break;
                    default:
                        break;
                }
            }

            OldValues = JsonConvert.SerializeObject(oldValues);
            NewValues = JsonConvert.SerializeObject(newValues);
            this.entry = entry;
        }

        public int Id { get; set; }
        public int AuditedEntityId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTimeOffset Creado { get; set; }
        public string CreadoPor { get; set; }
        public DateTimeOffset Actualizado { get; set; }
        public string ActualizadoPor { get; set; }


        public void UpdateIdentity() {
            AuditedEntityId = (int)entry.Property(PrimaryKeyName).CurrentValue;
        }
    }
}
