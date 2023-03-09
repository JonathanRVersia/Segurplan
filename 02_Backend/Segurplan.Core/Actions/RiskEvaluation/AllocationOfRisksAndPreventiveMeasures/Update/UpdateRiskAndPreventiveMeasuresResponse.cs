namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Update {
    public class UpdateRiskAndPreventiveMeasuresResponse {

        public UpdateRiskAndPreventiveMeasuresResponse(int riskPreventiveMeasureId) {
            RiskPreventiveMeasureId = riskPreventiveMeasureId;
        }

        public int RiskPreventiveMeasureId { get; set; }
    }
}
