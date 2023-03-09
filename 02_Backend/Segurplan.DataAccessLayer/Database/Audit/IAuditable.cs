using System;

namespace Segurplan.DataAccessLayer.Database.Audit {
    public interface IAuditable {
        DateTimeOffset Creado { get; set; }
        string CreadoPor { get; set; }

        DateTimeOffset Actualizado { get; set; }

        string ActualizadoPor { get; set; }
    }
}
