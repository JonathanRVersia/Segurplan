using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models {
    public class ActivityDropdownDto {
        public int Id { get; set; }

        //public string Description { get; set; }
        public int Number { get; set; }

        public string Title { get; set; }

        public int SubChapterId { get; set; }
    }
}
