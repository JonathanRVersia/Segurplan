using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View.PlanesDropdowns {

    public class ViewFamiliesPlanesDropdownsResponse {

        public List<FamiliesPlanesDropdowns> families;

        public ViewFamiliesPlanesDropdownsResponse(List<FamiliesPlanesDropdowns> result) {
            families = result;
        }
    }

    public class FamiliesPlanesDropdowns {

        public int Id { get; set; }

        public string Family { get; set; }
    }
}
