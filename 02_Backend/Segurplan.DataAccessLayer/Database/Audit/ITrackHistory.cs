using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Segurplan.DataAccessLayer.Database.Audit {
    public interface ITracksHistory {
        HistoryEntry CreateHistoryEntry(EntityEntry<ITracksHistory> entry);
    }

    public interface ITracksHistory<THistoryEntry> : ITracksHistory
        where THistoryEntry : HistoryEntry {
        new THistoryEntry CreateHistoryEntry(EntityEntry<ITracksHistory> entry);
        IEnumerable<THistoryEntry> HistoryEntries { get; set; }
    }
}
