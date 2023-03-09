using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Delegaciones
    {
        public Delegaciones()
        {
            Centros = new HashSet<Centros>();
        }

        public int Id { get; set; }
        public int? Codigo { get; set; }
        public string Delegacion { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }

        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Centros> Centros { get; set; }
    }
}
