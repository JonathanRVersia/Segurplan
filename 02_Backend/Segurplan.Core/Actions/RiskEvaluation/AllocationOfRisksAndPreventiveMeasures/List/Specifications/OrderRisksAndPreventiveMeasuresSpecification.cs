using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List.Specifications {
    public class OrderRisksAndPreventiveMeasuresSpecification : Specification<ListRisksAndPreventiveMeasuresResponse.ListItem> {
        public void ByChapterNumber() => OrderBy(c => c.ChapterNumber);
        public void ByChapterNumberDesc() => ApplyOrderByDescending(c => c.ChapterNumber);

        public void BySubChapterNumber() => OrderBy(c => c.SubChapterNumber);
        public void BySubChapterNumberDesc() => ApplyOrderByDescending(c => c.SubChapterNumber);


        public void ByActivityNumber() => OrderBy(c => c.ActivityNumber);
        public void ByActivityNumberDesc() => ApplyOrderByDescending(c => c.ActivityNumber);

        public void ByActivityDescription() => OrderBy(c => c.ActivityDescription);
        public void ByActivityDescriptionDesc() => ApplyOrderByDescending(c => c.ActivityDescription);

        public void ByRisks() => OrderBy(c => c.RiskName);
        public void ByRisksDesc() => ApplyOrderByDescending(c => c.RiskName);

        //public void ByPreventiveMeasureDescription() => OrderBy(c => c.PreventiveMeasureDescription);
        //public void ByPreventiveMeasureDescriptionDesc() => ApplyOrderByDescending(c => c.PreventiveMeasureDescription);

        public void ByProbability() => OrderBy(c => c.ProbabilityValue);
        public void ByProbabilityDesc() => ApplyOrderByDescending(c => c.ProbabilityValue);

        public void BySeriousness() => OrderBy(c => c.SeriousnessValue);
        public void BySeriousnessDesc() => ApplyOrderByDescending(c => c.SeriousnessValue);

        public void ByRiskLevel() => OrderBy(c => c.RiskLevelValue);
        public void ByRiskLevelDesc() => ApplyOrderByDescending(c => c.RiskLevelValue);
    }
}
