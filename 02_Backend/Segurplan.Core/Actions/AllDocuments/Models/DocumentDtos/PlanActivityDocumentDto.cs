using System.Collections.Generic;

namespace Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos {
    public class PlanActivityDocumentDto {

        public int Id { get; set; }

        public int Position { get; set; }

        public int Number { get; set; }

        public string ActivityIndex { get; set; }

        public string Description { get; set; } = "";

        public string WordDescriptionHtml { get; set; } = "";

        public bool IsSelected { get; set; }

        public int SubChapterPosition { get; set; }

        public int ChapterPosition { get; set; }

        public string WorkDetailsHtml { get; set; } = "";

        public bool WorkOrganizationRender { get; set; }

        public string WorkOrganizationHtml { get; set; } = "";

        public string RiskEvaluation { get; set; }

        public string MachineToolHtml { get; set; } = "";

        public bool AssociatedDetailsRender { get; set; }
        
        public string AssociatedDetails { get; set; } = "";

        public bool ActivityRisksRender { get; set; }

        public bool HasWorkDetails { get; set; }

        public bool HasWordDescription { get; set; }

        public int? RelationsId { get; set; }

        public List<PlanFormatoSinTablasActivityRisks> ActivityRisks { get; set; } = new List<PlanFormatoSinTablasActivityRisks>();

        public List<MeasuresPerRiskAndActivity> MeasuresPerRiskAndActivityHtml { get; set; } = new List<MeasuresPerRiskAndActivity>();
    }
}
