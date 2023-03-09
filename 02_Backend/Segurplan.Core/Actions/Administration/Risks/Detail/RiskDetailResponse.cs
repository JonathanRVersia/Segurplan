using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Risks.Detail {
    public class RiskDetailResponse {
        public RiskDetailResponse(ApplicationRisk risk) {
            Risk = risk;
        }
        public ApplicationRisk Risk { get; set; }
    }
}
