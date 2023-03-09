using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.DependantDropdowns {
    public class RiskAndPreventiveMeasuresDependantDropdownsByIdResponse {

        public List<SubChapterDropdownDto> SubChapter;
        public List<ActivityDropdownDto> Activities;
        public List<SubChapterDropdownDto> SubChapterVersion;
        public List<ChapterVersionDto> ChapterVersion;
        //public List<RiskDropdownDto> Risks;
        public List<PreventiveMeasureDetailDto> PreventiveMeasures;
        public bool Vigente; 


        public RiskAndPreventiveMeasuresDependantDropdownsByIdResponse(List<SubChapterDropdownDto> subChapter, List<ActivityDropdownDto> activities, List<SubChapterDropdownDto> subChapterVersion, List<ChapterVersionDto> chapterVersion, bool vigente/*,List<RiskDropdownDto> risks*/, List<PreventiveMeasureDetailDto> preventiveMeasures) {
            SubChapter = subChapter;
            Activities = activities;
            SubChapterVersion = subChapterVersion;
            ChapterVersion = chapterVersion;
            Vigente = vigente;
            //Risks = risks;
            PreventiveMeasures = preventiveMeasures;
        }
    }
}
