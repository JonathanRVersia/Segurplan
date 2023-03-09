using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class DireccionesNegocio
    {
        public DireccionesNegocio()
        {
            Centros = new HashSet<Centros>();
            Usuarios = new HashSet<Usuarios>();
        }

        public int Id { get; set; }
        public int? Codigo { get; set; }
        public string Direccion { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }

        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Centros> Centros { get; set; }
        public ICollection<Usuarios> Usuarios { get; set; }
    }
}
