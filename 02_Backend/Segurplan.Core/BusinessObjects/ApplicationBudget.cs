using System.Collections.Generic;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationBudget {

        public int Id { get; set; }

        public decimal Total { get; set; }

        public int ApplicabePercentage { get; set; }

        public decimal StudyBudget { get; set; }

        public decimal Difference { get; set; }

        public List<ApplicationArticle> SelectedArticles { get; set; }
    }
}
