using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList.Dtos {
    public class RisksAndPreventiveMeasuresTableDataModel {
        public int Id { get; set; }

        public int ChapterId { get; set; }
        public int ChapterNumber { get; set; }
        public int SubChapterId { get; set; }
        public int SubChapterNumber { get; set; }

        public int ActivityId { get; set; }
        public int ActivityNumber { get; set; }
        public string ActivityDescription { get; set; }

        public List<PreventiveMeasureModel> PreventiveMeasure { get; set; }


        public string RiskLevel { get; set; }
        public string TrafficLightsColour { get; set; }

        public string SeriousnessValue { get; set; }
        public string ProbabilityValue { get; set; }

        public int? RiskId { get; set; }
        public string RiskName { get; set; }
    }
}
