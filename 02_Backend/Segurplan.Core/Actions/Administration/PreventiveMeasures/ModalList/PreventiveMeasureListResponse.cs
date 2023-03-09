using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Segurplan.FrameworkExtensions.EntityFramework.Query;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.PreventiveMeasures.ModalList {
    public class PreventiveMeasureListResponse : IPaginable {
        public PreventiveMeasureListResponse(QueryResult<ListItem> preventiveMeasures) {
            this.PreventiveMeasures = preventiveMeasures?.Results.ToList() ?? new List<ListItem>();

            IsPaginated = preventiveMeasures.IsPaginated;
            Page = preventiveMeasures.Page;
            PageSize = preventiveMeasures.PageSize;
            SkippedRows = preventiveMeasures.SkippedRows;
            TotalCount = preventiveMeasures.TotalCount;
        }

        public List<ListItem> PreventiveMeasures;
        public bool IsPaginated { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? SkippedRows { get; set; }
        public int? TotalCount { get; set; }

        public class ListItem {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }

        }
    }
}
