using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Domain.Documents;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Generate.Plan {
    public class GeneratePlanResponse {
        public ProcesedDocument Document;

        public GeneratePlanResponse(ProcesedDocument document) {
            Document = document;
        }
    }
}
