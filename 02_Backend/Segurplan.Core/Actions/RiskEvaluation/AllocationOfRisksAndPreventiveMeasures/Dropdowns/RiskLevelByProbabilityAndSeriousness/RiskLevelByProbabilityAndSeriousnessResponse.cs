using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.RiskLevelByProbabilityAndSeriousness {
    public class RiskLevelByProbabilityAndSeriousnessResponse {
        public RiskLevelByProbabilityAndSeriousnessResponse(List<RiskLevelBySeriousnessAndProbabilitiesDto> riskLevelBySeriousnessAndProbabilities) {
            RiskLevelBySeriousnessAndProbabilities = riskLevelBySeriousnessAndProbabilities;
        }

        public List<RiskLevelBySeriousnessAndProbabilitiesDto> RiskLevelBySeriousnessAndProbabilities { get; }
    }
}
