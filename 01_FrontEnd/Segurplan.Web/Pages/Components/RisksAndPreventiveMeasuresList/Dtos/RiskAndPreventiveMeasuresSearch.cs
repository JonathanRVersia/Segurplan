using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List.Specifications;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;

namespace Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList.Dtos {
    public class RiskAndPreventiveMeasuresSearch {
        public int? ChapterNumber { get; set; }
        public int? SubChapterNumber { get; set; }
        public int? ActivityNumber { get; set; }
        public int? RiskCode { get; set; }
        public int? MeasureCode { get; set; }

        public int? ChapterId { get; set; }
        public List<int> SubchapterIdList { get; set; }
        public int? SubChapterId { get; set; }
        public int? ActivityId { get; set; }
        public int? RiskId { get; set; }
        public int? MeasureId { get; set; }
        public string MeasureDescription { get; set; }
        public bool IsBorrador { get; set; }
        public bool BorradorExist { get; set; }

        public List<ChaptSubChaptActFilterData> ChapSubChaptFilter = new List<ChaptSubChaptActFilterData>();

        public FilterRisksAndPreventiveMeasuresSpecification ToSpecification() {

            var searchFilter = new FilterRisksAndPreventiveMeasuresSpecification();

            if (IsBorrador && !BorradorExist)
                ChapterId = 0;

            if (ChapSubChaptFilter.Any())
                searchFilter.ChaptSubChaptActIds(ChapSubChaptFilter);

            if (ChapterId != null)
                searchFilter.ByChapterId(ChapterId);

            if (SubchapterIdList != null && SubchapterIdList.Count > 0) {
                searchFilter.BySubChapterIdList(SubchapterIdList);
            }

            if (SubChapterId != null)
                searchFilter.BySubChapterId(SubChapterId);

            if (ActivityId != null)
                searchFilter.ByActivityId(ActivityId);
                
            if (RiskId != null)
                searchFilter.ByRiskId(RiskId);

            if (!string.IsNullOrEmpty(MeasureDescription))
                searchFilter.ByMeasureDescription(MeasureDescription);


            return searchFilter;
        }

    }
}
