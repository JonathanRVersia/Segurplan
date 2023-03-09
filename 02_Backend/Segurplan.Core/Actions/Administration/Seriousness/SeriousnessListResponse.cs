using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Seriousness {
    public class SeriousnessListResponse {
        public List<ApplicationSeriousness> SeriousnessList { get; set; } = new List<ApplicationSeriousness>();

        public int TotalRows { get; set; }

        public SeriousnessListResponse(List<ApplicationSeriousness> seriousnessList, int totalRows) {
            SeriousnessList = seriousnessList;
            TotalRows = totalRows;
        }
    }
}
