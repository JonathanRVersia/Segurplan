using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Seriousness.Detail {
    public class GetSeriousnessDetailResponse {
        public GetSeriousnessDetailResponse(ApplicationSeriousness seriousness) {
            Seriousness = seriousness;
        }

        public ApplicationSeriousness Seriousness { get; set; }
    }
}
