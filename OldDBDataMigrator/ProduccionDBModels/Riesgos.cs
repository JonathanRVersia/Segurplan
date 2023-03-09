using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Riesgos
    {
        public Riesgos()
        {
            Evaluaciones = new HashSet<Evaluaciones>();
            EvaluacionesMedida = new HashSet<EvaluacionesMedida>();
        }

        public int Id { get; set; }
        public int? Codigo { get; set; }
        public string Riesgo { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEstado { get; set; }
        public int? IdNuevo { get; set; }
        public int? IdActividadRiesgo { get; set; }

        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Evaluaciones> Evaluaciones { get; set; }
        public ICollection<EvaluacionesMedida> EvaluacionesMedida { get; set; }
    }
}
