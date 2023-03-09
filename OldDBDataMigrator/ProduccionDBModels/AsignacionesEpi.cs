using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class AsignacionesEpi
    {
        public int Id { get; set; }
        public int? IdCapitulo { get; set; }
        public int? IdSubcapitulo { get; set; }
        public int? IdActividad { get; set; }
        public int? IdEpi { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEstado { get; set; }
        public int? IdNuevo { get; set; }
        public string DescripcionWord { get; set; }
        public int? OrdenEpi { get; set; }

        public Actividades IdActividadNavigation { get; set; }
        public Capitulos IdCapituloNavigation { get; set; }
        public Subcapitulos IdSubcapituloNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
    }
}
