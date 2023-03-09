using System.Collections.Generic;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.List {
    public class RiskAndPreventiveMeasuresListDropdownResponse {
        public List<ChapterDropdownDto> Chapter;
        public List<SubChapterDropdownDto> SubChapterCurrent;
        public List<ActivityDropdownDto> ActivityCurrent;
        public List<RiskDropdownDto> Risk;
        public List<PreventiveMeasureDetailDto> Measure;
        public List<int> SubchapterIdList;
        public int SubChapterId;
        public int ActivityId;
        public bool BorradorExist;

        public RiskAndPreventiveMeasuresListDropdownResponse(
            List<ChapterDropdownDto> chapter,
            List<SubChapterDropdownDto> subChapterCurrent,
            List<ActivityDropdownDto> activityCurrent,
            List<RiskDropdownDto> risk,
            List<PreventiveMeasureDetailDto> measure,
            List<int> subchapterIdList,
            int subchapterId,
            int activityId,
            bool borradorExist
            ) {
            Chapter = chapter;
            SubChapterCurrent = subChapterCurrent;
            ActivityCurrent = activityCurrent;
            Risk = risk;
            Measure = measure;
            SubchapterIdList = subchapterIdList;
            SubChapterId = subchapterId;
            ActivityId = activityId;
            BorradorExist = borradorExist;
        }


    }
}
