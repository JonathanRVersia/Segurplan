using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Family.Detail {
    public class FamilyDetailResponse {
        public ApplicationFamily Family { get; set; }
        public FamilyDetailResponse(ApplicationFamily family) {
            Family = family;
        }
    }
}
