using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models {
    public class PreventiveMeasureDetailDto {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Description { get; set; }
        public int Order { get; set; }
    }
}
