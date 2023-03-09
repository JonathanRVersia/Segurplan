using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Gravedades
    {
        public Gravedades()
        {
            Evaluaciones = new HashSet<Evaluaciones>();
            NrGrPrRelaciones = new HashSet<NrGrPrRelaciones>();
        }

        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Gravedad { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? Orden { get; set; }

        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Evaluaciones> Evaluaciones { get; set; }
        public ICollection<NrGrPrRelaciones> NrGrPrRelaciones { get; set; }
    }
}
