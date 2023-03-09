using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.FrameworkExtensions.EntityFramework.Query;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List {
    public class ListRisksAndPreventiveMeasuresResponse : IPaginable {

        public ListRisksAndPreventiveMeasuresResponse(QueryResult<ListItem> riskAndPrevMeasures = null) {

            RiskAndPrevMeasures = riskAndPrevMeasures?.Results.ToList() ?? new List<ListItem>();

            IsPaginated = riskAndPrevMeasures.IsPaginated;
            Page = riskAndPrevMeasures.Page;
            PageSize = riskAndPrevMeasures.PageSize;
            SkippedRows = riskAndPrevMeasures.SkippedRows;
            TotalCount = riskAndPrevMeasures.TotalCount;
        }


        public List<ListItem> RiskAndPrevMeasures;
        public bool IsPaginated { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? SkippedRows { get; set; }
        public int? TotalCount { get; set; }


        public class ListItem : Segurplan.DataAccessLayer.Database.DataTransferObjects.AuditableTableBase {
            public int Id { get; set; }
            public int ChapterId { get; set; }
            public int ChapterNumber { get; set; }
            public int SubChapterId { get; set; }
            public int SubChapterNumber { get; set; }
            public int ActivityId { get; set; }
            public int ActivityNumber { get; set; }
            public string ActivityDescription { get; set; }
            public int? RiskId { get; set; }
            public string RiskName { get; set; }
            public string RiskLevelTrafficLightsColour { get; set; }

            public List<PreventiveMeasureListDto> PreventiveMeasures { get; set; }

            public string ProbabilityValue { get; set; }
            public string SeriousnessValue { get; set; }
            public string RiskLevelLevel { get; set; }
            public int RiskLevelValue { get; set; }
            public int RiskOrder { get; set; }
        }
    }


}
