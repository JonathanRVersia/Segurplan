using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos {
    public class PlanBudgetDocumentDto {
        public PlanBudgetDocumentDto() {
            ArticleFamily = new List<ApplicationArticleFamily>();
        }
        public List<ApplicationArticleFamily> ArticleFamily { get; set; }

    }
}
