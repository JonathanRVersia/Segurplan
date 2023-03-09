using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.DuplicatePlanCustomChapters {
    public class DuplicatePlanCustomChaptersResponse {

        public DuplicatePlanCustomChaptersResponse(List<PlanActivityVersion> updatedPlanActivityVersions) {
            UpdatedPlanActivityVersions = updatedPlanActivityVersions;
        }

        public List<PlanActivityVersion> UpdatedPlanActivityVersions;        
    }
}
