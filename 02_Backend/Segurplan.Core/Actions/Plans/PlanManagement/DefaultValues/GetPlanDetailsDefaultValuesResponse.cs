using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Plans.PlanManagement.DefaultValues {
    public class GetPlanDetailsDefaultValuesResponse {
        
        public List<PlanDetailsDefaultValuesDto> DefaultValues;

        public GetPlanDetailsDefaultValuesResponse(List<PlanDetailsDefaultValuesDto> defaultValues) {
            this.DefaultValues = defaultValues;
        }
    }
}
