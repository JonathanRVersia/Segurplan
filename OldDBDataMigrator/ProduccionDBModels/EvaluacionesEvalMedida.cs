using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class EvaluacionesEvalMedida
    {
        public int Id { get; set; }
        public int? IdEvaluacionMedida { get; set; }
        public int? IdEvaluacion { get; set; }
        public string ModoImportar { get; set; }

        public Evaluaciones IdEvaluacionNavigation { get; set; }
    }
}
