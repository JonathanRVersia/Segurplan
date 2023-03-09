using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View {
    public class ViewPlanPlanesResponse {

        public List<ApplicationPlaneFamily> PlaneList { get; set; } = new List<ApplicationPlaneFamily>();

        public List<SafetyPlanPlane> PlanPlaneList { get; set; } = new List<SafetyPlanPlane>();
    }
}
