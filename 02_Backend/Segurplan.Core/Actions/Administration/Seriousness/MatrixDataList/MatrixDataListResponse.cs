using System.Collections.Generic;

namespace Segurplan.Core.Actions.Administration.Seriousness.MatrixDataList {
    public class MatrixDataListResponse {
        public MatrixDataListResponse(List<MatrixProbabilityDTO> probability) {
            Probability = probability;
        }

        public MatrixDataListResponse(List<MatrixProbabilityDTO> probability, List<MatrixRiskLevelDTO> riskLevel) : this(probability) {
            RiskLevel = riskLevel;
        }

        public List<MatrixProbabilityDTO> Probability { get; }
        public List<MatrixRiskLevelDTO> RiskLevel { get; }
    }
}
