using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class TiposArticulo
    {
        public TiposArticulo()
        {
            Articulos = new HashSet<Articulos>();
            PlanesPresupuestos = new HashSet<PlanesPresupuestos>();
        }

        public int Id { get; set; }
        public int? Codigo { get; set; }
        public string TipoArticulo { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }

        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Articulos> Articulos { get; set; }
        public ICollection<PlanesPresupuestos> PlanesPresupuestos { get; set; }
    }
}
