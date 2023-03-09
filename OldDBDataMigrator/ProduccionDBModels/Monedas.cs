using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Monedas
    {
        public Monedas()
        {
            Articulos = new HashSet<Articulos>();
            PlanesPresupuestos = new HashSet<PlanesPresupuestos>();
        }

        public int Id { get; set; }
        public string Moneda { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }

        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Articulos> Articulos { get; set; }
        public ICollection<PlanesPresupuestos> PlanesPresupuestos { get; set; }
    }
}
