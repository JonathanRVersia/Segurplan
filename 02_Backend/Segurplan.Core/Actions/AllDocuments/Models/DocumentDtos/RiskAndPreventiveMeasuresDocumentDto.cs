using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos {
    public class RiskAndPreventiveMeasuresDocumentDto: AuditableTableBase {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public int ChapterNumber { get; set; }
        public int SubChapterId { get; set; }
        public int SubChapterNumber { get; set; }
        public int ActivityId { get; set; }
        public int ActivityNumber { get; set; }
        public string ActivityDescription { get; set; } = "";
        public int? RiskId { get; set; }
        public string RiskName { get; set; } = "";

        public List<PreventiveMeasureListDocumentDto> PreventiveMeasures { get; set; }

        public string ProbabilityValue { get; set; } = "";
        public string SeriousnessValue { get; set; } = "";
        public string RiskLevelLevel { get; set; } = "";
        public int RiskLevelValue { get; set; }
        public int RiskOrder { get; set; }

        public Seriousness MyProperty { get; set; }
    }
}
