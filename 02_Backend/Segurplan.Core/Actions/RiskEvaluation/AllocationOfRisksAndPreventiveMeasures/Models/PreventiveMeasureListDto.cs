using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models {
    public class PreventiveMeasureListDto {
        public int Id { get; set; }
        public string PreventiveMeasureDescription { get; set; }
        public int PreventiveMeasureOrder { get; set; }
    }
}
