using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Update {
    public class UpdatePlanRequestBase {

        public int UserId { get; set; }
        public SafetyPlan PlanInformation { get; set; }
    }
}
