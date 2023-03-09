using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Segurplan.FrameworkExtensions.EntityFramework.Query;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.Seriousness.ModalList {
    public class SeriousnessListResponse : IPaginable {
        public SeriousnessListResponse(QueryResult<ListItem> seriousness) {
            this.Seriousness = seriousness?.Results.ToList() ?? new List<ListItem>();

            IsPaginated = seriousness.IsPaginated;
            Page = seriousness.Page;
            PageSize = seriousness.PageSize;
            SkippedRows = seriousness.SkippedRows;
            TotalCount = seriousness.TotalCount;
        }

        public List<ListItem> Seriousness;
        public bool IsPaginated { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? SkippedRows { get; set; }
        public int? TotalCount { get; set; }

        public class ListItem {
            public int Id { get; set; }
            public string Value { get; set; }
        }
    }
}
