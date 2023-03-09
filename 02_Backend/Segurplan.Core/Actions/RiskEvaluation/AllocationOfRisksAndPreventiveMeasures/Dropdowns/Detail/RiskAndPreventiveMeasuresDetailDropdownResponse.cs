using System.Collections.Generic;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.Detail {
    public class RiskAndPreventiveMeasuresDetailDropdownResponse {
        public List<ChapterDropdownDto> Chapter;
        public List<SubChapterDropdownDto> SubChapter;
        public List<ActivityDropdownDto> Activity;
        public List<RiskDropdownDto> Risk;
        public List<ProbabilityDropownsDto> Probability;
        public List<SeriousnessDropdownDto> Seriousness;
        public List<ChapterVersionDto> ChapterVersion;


        public RiskAndPreventiveMeasuresDetailDropdownResponse(
            List<ChapterDropdownDto> chapter,
            List<RiskDropdownDto> risk,
            List<ProbabilityDropownsDto> probability,
            List<SeriousnessDropdownDto> seriousness,
            List<ChapterVersionDto> chapterVersion
            ) {
            Chapter = chapter;
            Risk = risk;
            Probability = probability;
            Seriousness = seriousness;
            ChapterVersion = chapterVersion;
        }
    }

}
