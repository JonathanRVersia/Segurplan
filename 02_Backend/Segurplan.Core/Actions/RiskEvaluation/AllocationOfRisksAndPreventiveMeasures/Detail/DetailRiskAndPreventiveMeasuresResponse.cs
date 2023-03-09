using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Detail {
    public class DetailRiskAndPreventiveMeasuresResponse {

        public int Id { get; set; }


        public int ChapterId { get; set; }
        public string ChapterTitle { get; set; }

        public int SubChapterId { get; set; }
        public string SubChapterTitle { get; set; }

        public int ActivityId { get; set; }
        public string ActivityDescription { get; set; }

        public int RiskId { get; set; }

        public int RiskCode { get; set; }

        public string RiskName { get; set; }

        public List<PreventiveMeasureDetailDto> PreventiveMeasures { get; set; }

        public int ProbabilityId { get; set; }
        public string ProbabilityValue { get; set; }

        public int SeriousnessId { get; set; }
        public string SeriousnessValue { get; set; }

        public int RiskLevelId { get; set; }
        public string RiskLevelLevel { get; set; }
        public int RiskOrder { get; set; }
        public DateTime CreateDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }

    }
}
