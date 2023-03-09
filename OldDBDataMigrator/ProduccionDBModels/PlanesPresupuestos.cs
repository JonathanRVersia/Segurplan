using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class PlanesPresupuestos
    {
        public int Id { get; set; }
        public int? IdPlan { get; set; }
        public int? IdArticulo { get; set; }
        public string Descripcion { get; set; }
        public double? Cantidad { get; set; }
        public double? PrecioUnitario { get; set; }
        public int? IdMoneda { get; set; }
        public double? NumUsos { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdTipoArticulo { get; set; }

        public Monedas IdMonedaNavigation { get; set; }
        public Planes IdPlanNavigation { get; set; }
        public TiposArticulo IdTipoArticuloNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
    }
}
