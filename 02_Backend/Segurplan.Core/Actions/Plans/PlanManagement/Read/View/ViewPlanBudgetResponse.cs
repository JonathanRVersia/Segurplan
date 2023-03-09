using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View {
    public class ViewPlanBudgetResponse {
        public List<ApplicationArticleFamily> ArticlesFamily { get; set; } = new List<ApplicationArticleFamily>();
        public List<ApplicationTask> ArticlesByTask { get; set; } = new List<ApplicationTask>();
        public List<ApplicationArticle> SelectedArticlesDB { get; set; } = new List<ApplicationArticle>();
        public int ApplicablePercentage { get; set; }
    }
}
