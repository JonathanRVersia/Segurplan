using System.Collections.Generic;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationArticleFamily {
        public int Id { get; set; }
        public string Family { get; set; }
        public decimal Price { get; set; }
        public string Number { get; set; }
        public List<ApplicationArticle> Articles { get; set; }
    }
}
