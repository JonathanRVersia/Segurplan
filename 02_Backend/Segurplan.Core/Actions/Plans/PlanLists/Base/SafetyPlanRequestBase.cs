using Segurplan.Core.Actions.Plans.PlansData;


namespace Segurplan.Core.Actions.Plans.PlanLists {
    public class SafetyPlanRequestBase {
        public Filter Filter { get; set; }
        public TableState TableState { get; set; }
        public string UserID { get; set; }
        public SafetyPlanRequestBase(TableState tablestate, Filter filter, string userID) {
            Filter = filter;
            TableState = tablestate;
            UserID = userID;
        }
    }
}
