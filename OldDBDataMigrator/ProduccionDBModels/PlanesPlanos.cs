using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class PlanesPlanos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Archivo { get; set; }
        public int? IdPlan { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdPlanoGeneral { get; set; }
        public int? Posicion { get; set; }

        public PlanosGenerales IdPlanoGeneralNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
    }
}
