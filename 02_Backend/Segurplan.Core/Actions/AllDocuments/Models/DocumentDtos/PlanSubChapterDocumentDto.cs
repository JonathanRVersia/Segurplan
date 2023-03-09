using System.Collections.Generic;

namespace Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos {
    public class PlanSubChapterDocumentDto {
        public int Id { get; set; }

        public int Position { get; set; }

        public int Number { get; set; }

        public string SubchapterIndex { get; set; }

        public string Title { get; set; }

        public string Description { get; set; } = "";

        public string WordDescriptionHtml { get; set; } = "";

        public bool BlueHeaderTableRender { get; set; }

        public bool IsSelected { get; set; }

        public string WorkDetailsHtml { get; set; } = "";

        public string WorkOrganizationHtml { get; set; } = "";

        public string RiskEvaluation { get; set; }

        public string MachineToolHtml { get; set; } = "";

        public string AssociatedDetails { get; set; }

        public List<PlanActivityDocumentDto> ActivitiesHtml { get; set; } = new List<PlanActivityDocumentDto>();
    }
}
