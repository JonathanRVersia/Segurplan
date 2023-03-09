namespace Segurplan.Core.Actions.Plans.PlanManagement.Read {
    public class ReadPlanGeneralDataRequest {

        public int PlanId { get; set; }

        public ReadPlanGeneralDataRequest(int planId) => PlanId = planId;

    }
}
