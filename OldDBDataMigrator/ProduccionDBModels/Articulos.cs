using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Articulos
    {
        public int Id { get; set; }
        public int? Codigo { get; set; }
        public string Articulo { get; set; }
        public int? IdTipoArticulo { get; set; }
        public double? PrecioBase { get; set; }
        public int? IdMoneda { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }

        public Monedas IdMonedaNavigation { get; set; }
        public TiposArticulo IdTipoArticuloNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
    }
}
