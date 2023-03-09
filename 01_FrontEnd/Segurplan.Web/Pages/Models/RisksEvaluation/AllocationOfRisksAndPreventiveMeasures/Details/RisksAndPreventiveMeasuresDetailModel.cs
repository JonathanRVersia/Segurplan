using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Segurplan.Web.Pages.Models.RisksEvaluation.AllocationOfRisksAndPreventiveMeasures.Details {
    public class RisksAndPreventiveMeasuresDetailModel {
        public int Id { get; set; }

        [Required]
        public int ChapterId { get; set; }
        public string ChapterTitle { get; set; }

        [Required]
        public int SubChapterId { get; set; }
        public string SubChapterTitle { get; set; }

        [Required]
        public int ActivityId { get; set; }
        public string ActivityDescription { get; set; }

        [Required]
        public int RiskId { get; set; }

        public int RiskCode { get; set; }

        public string RiskName { get; set; }

        public List<PreventiveMeasureModel> PreventiveMeasures { get; set; } = new List<PreventiveMeasureModel>();

        [Required]
        public int ProbabilityId { get; set; }
        public string ProbabilityValue { get; set; }

        [Required]
        public int SeriousnessId { get; set; }
        public string SeriousnessValue { get; set; }

        [Required]
        public int RiskLevelId { get; set; }
        public string RiskLevelLevel { get; set; }

        [Required]
        public int RiskOrder { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreateDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime UpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }

    public class PreventiveMeasureModel {
        public int PreventiveMeasureId { get; set; }
        public string PreventiveMeasureCode { get; set; }
        public string PreventiveMeasureDescription { get; set; }
        public int PreventiveMeasureOrder { get; set; }
    }
}
