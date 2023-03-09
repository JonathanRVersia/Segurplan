using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Evaluaciones
    {
        public Evaluaciones()
        {
            EvaluacionesEvalMedida = new HashSet<EvaluacionesEvalMedida>();
            EvaluacionesMedida = new HashSet<EvaluacionesMedida>();
        }

        public int Id { get; set; }
        public int? IdCapitulo { get; set; }
        public int? IdSubcapitulo { get; set; }
        public int? IdActividad { get; set; }
        public int? IdRiesgo { get; set; }
        public int? IdProbabilidad { get; set; }
        public int? IdGravedad { get; set; }
        public int? IdNivelRiesgo { get; set; }
        public int? OrdenRiesgo { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEstado { get; set; }
        public int? IdNuevo { get; set; }
        public string DescripcionWord { get; set; }

        public Actividades IdActividadNavigation { get; set; }
        public Capitulos IdCapituloNavigation { get; set; }
        public Gravedades IdGravedadNavigation { get; set; }
        public NivelesRiesgo IdNivelRiesgoNavigation { get; set; }
        public ProbabilidadesRiesgo IdProbabilidadNavigation { get; set; }
        public Riesgos IdRiesgoNavigation { get; set; }
        public Subcapitulos IdSubcapituloNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<EvaluacionesEvalMedida> EvaluacionesEvalMedida { get; set; }
        public ICollection<EvaluacionesMedida> EvaluacionesMedida { get; set; }
    }
}
