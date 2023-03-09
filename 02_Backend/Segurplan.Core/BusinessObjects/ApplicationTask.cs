using System;
using System.Collections.Generic;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationTask {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ApplicationArticleTaskDetail> TaskDetails { get; set; }
        public List<ApplicationArticle> Articles { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
