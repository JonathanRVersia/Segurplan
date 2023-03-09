using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos {
    public class PlanFormatoSinTablasActivityRisks {

        public string RiskName { get; set; } = "";
        public string ProbabilityValue { get; set; } = "";
        public string SeriousnessValue { get; set; } = "";
        public string RiskLevelLevel { get; set; } = "";

        public string ProbabilityChar { get; set; } = "";
        public string SeriousnessChar { get; set; } = "";
        public string RiskLevelChar { get; set; } = "";
    }
}
