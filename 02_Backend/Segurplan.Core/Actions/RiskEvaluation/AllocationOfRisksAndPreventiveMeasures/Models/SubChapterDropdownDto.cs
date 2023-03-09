using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models {
    public class SubChapterDropdownDto {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public int IdChapter { get; set; }
        public string IdSubchapter { get; set; }
    }
}
