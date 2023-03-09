using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos;

namespace Segurplan.Core.Actions.RiskEvaluation.EvaluationsOfRisksAndPreventiveMeasures.Generate.Models {
    public class GenerateRiskEvaluationModel {

        public List<RiskAndPreventiveMeasuresDocumentDto> riskAndPreventiveMeasuresDto;
        public List<PlanChapterDocumentDto> planChapterDto;

        public GenerateRiskEvaluationModel(List<RiskAndPreventiveMeasuresDocumentDto> riskAndPreventiveMeasuresDto, List<PlanChapterDocumentDto> planChapterDto) {
            this.riskAndPreventiveMeasuresDto = riskAndPreventiveMeasuresDto;
            this.planChapterDto = planChapterDto;
        }
    }
}
