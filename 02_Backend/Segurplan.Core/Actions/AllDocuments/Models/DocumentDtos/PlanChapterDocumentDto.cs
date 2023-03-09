using System;
using System.Collections.Generic;

namespace Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos {
    public class PlanChapterDocumentDto {
        public int Id { get; set; }

        public int Position { get; set; }

        public int Number { get; set; }

        public string ChapterIndex { get; set; }

        public string Title { get; set; } = "";

        public string WordDescriptionHtml { get; set; } = "";

        public bool IsSelected { get; set; }

        public string WorkDetailsHtml { get; set; } = "";

        public string WorkOrganizationHtml { get; set; } = "";

        public string RiskEvaluation { get; set; }

        public string MachineToolHtml { get; set; } = "";

        public string AssociatedDetails { get; set; }

        public bool WorkDescriptionRender { get; set; }

        public DateTime? ApprovementDate { get; set; }

        public DateTime CreateDate { get; set; }

        public int VersionNumber { get; set; }

        public int IdApprover { get; set; }

        public int IdReviewer { get; set; }

        public int CreatedBy { get; set; }

        public List<PlanSubChapterDocumentDto> SubChaptersHtml { get; set; } = new List<PlanSubChapterDocumentDto>();
        public bool DefaultSelectedChapter { get; set; }

        public List<RiskAndPreventiveMeasuresDocumentDto> RisksAndPreventiveMeasures { get; set; } = new List<RiskAndPreventiveMeasuresDocumentDto>();
        public bool ActivityRisksRender { get; set; }
    }
}
