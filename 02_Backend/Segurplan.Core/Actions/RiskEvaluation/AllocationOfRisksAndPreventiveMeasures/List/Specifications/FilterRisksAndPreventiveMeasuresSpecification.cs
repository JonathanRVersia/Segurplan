using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List.Specifications {
    public class FilterRisksAndPreventiveMeasuresSpecification : Specification<ListRisksAndPreventiveMeasuresResponse.ListItem> {

        public void ById(int? id) => Criteria(s => s.Id == id);
        public void ByChapterId(int? id) => Criteria(s => s.ChapterId == id);
        public void BySubChapterId(int? id) => Criteria(s => s.SubChapterId == id);
        public void BySubChapterIdList(List<int> idList) => Criteria(s => idList.Contains(s.SubChapterId));
        public void ByActivityId(int? id) => Criteria(s => s.ActivityId == id);
        public void ByRiskId(int? riskId) => Criteria(s => s.RiskId == riskId);
        public void ByMeasureId(int? measureId) => Criteria(s => s.PreventiveMeasures.Any(pm => pm.Id == measureId.Value));
        public void ByMeasureDescription(string description) => Criteria(s => s.PreventiveMeasures.Any(pm => pm.PreventiveMeasureDescription == description));

        public void ChaptSubChaptActIds(int chapterId, int subChapterId, int activityId) => Criteria(s => s.ChapterId == chapterId && s.SubChapterId == subChapterId && s.ActivityId == activityId);
        public void ChaptSubChaptActIds(List<ChaptSubChaptActFilterData> ids) => Criteria(s => ids.Select(x => x.ChapterId).Contains(s.ChapterId)
                                                                                            &&
                                                                                            ids.Select(x => x.SubChapterId).Contains(s.SubChapterId)
                                                                                            &&
                                                                                            ids.Select(x => x.ActivityId).Contains(s.ActivityId));

    }


}
