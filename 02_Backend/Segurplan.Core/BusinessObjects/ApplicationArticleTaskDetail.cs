using System;
using System.Collections.Generic;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationArticleTaskDetail {

        public int Id { get; set; }
        public ApplicationArticle Article { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
