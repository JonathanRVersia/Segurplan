using System;

namespace Segurplan.Core.Domain.Audit {
    public interface IAuditable {
        DateTimeOffset Creado { get; set; }
        string CreadoPor { get; set; }

        DateTimeOffset Actualizado { get; set; }

        string ActualizadoPor { get; set; }
    }
}
