using System.Collections.Generic;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Dropdowns.UpdateAffiliatedDependantData {
    public class UpdateAffiliatedDependantDataResponse {
        public List<SelectDataDto> FilteredDelegations { get; set; } = new List<SelectDataDto>();
    }
}
