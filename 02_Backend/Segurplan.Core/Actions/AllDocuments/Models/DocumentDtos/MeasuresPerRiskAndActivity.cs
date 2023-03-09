using System.Collections.Generic;

namespace Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos {
    public class MeasuresPerRiskAndActivity {

        public int Id { get; set; }

        public string ActivityName { get; set; }

        public string RiskName { get; set; } = "";

        public string ProbabilityValue { get; set; } = "";

        public string SeriousnessValue { get; set; } = "";

        public string RiskLevelLevel { get; set; } = "";

        public string ProbabilityChar { get; set; } = "";

        public string SeriousnessChar { get; set; } = "";

        public string RiskLevelChar { get; set; } = "";

        public string WordDescriptionHtml { get; set; } = "";

        public int RiskOrder { get; set; }

        public List<PreventiveMeasureListDocumentDto> PreventiveMeasuresHtml { get; set; } = new List<PreventiveMeasureListDocumentDto>();
    }
}
