using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models {
    public class RiskLevelBySeriousnessAndProbabilitiesDto {

        public int Id { get; set; }

        public int SeriousnessId { get; set; }
        public string SeriousnessValue { get; set; }
        public int ProbabilityId { get; set; }
        public string ProbabilityValue { get; set; }
        public int RiskLevelId { get; set; }
        public string RiskLevelLevel { get; set; }

    }
}
