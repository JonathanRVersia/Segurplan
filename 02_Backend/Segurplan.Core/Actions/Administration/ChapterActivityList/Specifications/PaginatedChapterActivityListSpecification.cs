using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.ChapterActivityList.Specifications {
    public class PaginatedChapterActivityListSpecification : Specification<ChapterActivityListResponse.ListItem> {
        public PaginatedChapterActivityListSpecification(int page, int pageSize) {
            Paginated(page * pageSize, pageSize);
        }
    }
}
