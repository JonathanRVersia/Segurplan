using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Web.Pages.Models.Budget {
    public class BudgetViewModel {
        public List<ApplicationArticleFamily> ArticlesByFamily { get; set; } = new List<ApplicationArticleFamily>();
        public List<ApplicationTask> ArticlesByTask { get; set; } = new List<ApplicationTask>();
        public List<ApplicationArticle> SelectedArticlesDB { get; set; } = new List<ApplicationArticle>();
        public int ApplicablePercentage { get; set; } 
    }
}
