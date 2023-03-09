using System;
using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Risks {
    public class RiskListResponse {
        public List<ApplicationRisk> RiskList { get; set; } = new List<ApplicationRisk>();

        public int TotalRows { get; set; }

        public RiskListResponse(List<ApplicationRisk> riskList, int totalRows) {
            this.RiskList = riskList;
            this.TotalRows = totalRows;
        }
    }
}
