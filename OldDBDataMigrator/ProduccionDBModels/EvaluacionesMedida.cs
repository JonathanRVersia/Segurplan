using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class EvaluacionesMedida
    {
        public int Id { get; set; }
        public int? IdCapitulo { get; set; }
        public int? IdSubcapitulo { get; set; }
        public int? IdActividad { get; set; }
        public int? IdRiesgo { get; set; }
        public int? IdMedida { get; set; }
        public int? IdEvaluacion { get; set; }
        public int? OrdenMedida { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificación { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEstado { get; set; }
        public int? IdNuevo { get; set; }
        public int? IdCapImp { get; set; }
        public int? IdSubImp { get; set; }
        public int? IdActImp { get; set; }
        public string Csa { get; set; }
        public int? Idmodificacion { get; set; }

        public Actividades IdActividadNavigation { get; set; }
        public Capitulos IdCapituloNavigation { get; set; }
        public Evaluaciones IdEvaluacionNavigation { get; set; }
        public Medidas IdMedidaNavigation { get; set; }
        public Riesgos IdRiesgoNavigation { get; set; }
        public Subcapitulos IdSubcapituloNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
    }
}
