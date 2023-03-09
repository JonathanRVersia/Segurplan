using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Plans.PlanLists {
    public class SafetyPlanResponseBase {
        public List<SafetyPlan> ListaPlanes { get; }
        public int TotalRows { get; set; }
        public List<int> MyRoleIDs { get; set; }
        public SafetyPlanResponseBase(List<SafetyPlan> listaPlanes, int totalRows, List<int> myRoleIDs) {
            ListaPlanes = listaPlanes;
            TotalRows = totalRows;
            MyRoleIDs = myRoleIDs;
        }
    }
}
