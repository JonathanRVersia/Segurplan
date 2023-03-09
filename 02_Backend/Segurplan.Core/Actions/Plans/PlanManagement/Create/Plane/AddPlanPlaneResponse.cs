using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Create.Plane {
    public class AddPlanPlaneResponse {
        public SafetyPlanPlane safetyPlanPlane;

        public AddPlanPlaneResponse(SafetyPlanPlane safetyPlanPlane) {
            this.safetyPlanPlane = safetyPlanPlane;
        }
    }
}
