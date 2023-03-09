using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models {
    public class UpdateRiskAndPreventiveMeasuresModel {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public int SubChapterId { get; set; }
        public int ActivityId { get; set; }
        public int RiskId { get; set; }
        public int ProbabilityId { get; set; }
        public int SeriousnessId { get; set; }
        public int RiskLevelId { get; set; }
        public int RiskOrder { get; set; }
        public List<UpdatePreventiveMeasure> PreventiveMeasures { get; set; } = new List<UpdatePreventiveMeasure>();
    }

    public class UpdatePreventiveMeasure {
        public int Id { get; set; }
        public int Order { get; set; }
    }
}
