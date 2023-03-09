using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Plantillas
    {
        public Plantillas()
        {
            Planes = new HashSet<Planes>();
        }

        public int Id { get; set; }
        public string Plantilla { get; set; }
        public string Archivo { get; set; }
        public DateTime? FecModificacion { get; set; }
        public DateTime? FecCreacion { get; set; }
        public int? IdUsuario { get; set; }
        public string Descripcion { get; set; }
        public int? TipoSel { get; set; }

        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Planes> Planes { get; set; }
    }
}
