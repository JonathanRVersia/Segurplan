using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class ClasificacionesPlan
    {
        public ClasificacionesPlan()
        {
            Planes = new HashSet<Planes>();
        }

        public int Id { get; set; }
        public string Clasificacion { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }

        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Planes> Planes { get; set; }
    }
}
