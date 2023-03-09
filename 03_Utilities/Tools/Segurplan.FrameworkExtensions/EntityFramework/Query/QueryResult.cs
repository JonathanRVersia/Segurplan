using System.Collections.Generic;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.FrameworkExtensions.EntityFramework.Query {
    public class QueryResult<T> : IPaginable {
        public IEnumerable<T> Results { get; set; }

        public bool IsPaginated { get; set; }

        public int? TotalCount { get; set; }

        public int? SkippedRows { get; set; }

        public int? Page => (SkippedRows / PageSize) + 1;

        public int? PageSize { get; set; }
    }
}
