using System;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class ArticleTaskDetail: AuditableTableBase {
        public int Id { get; set; }
        public int IdArticle { get; set; }
        public int IdTasks { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("ArticleTaskDetailCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("ArticleTaskDetailModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        [ForeignKey("IdArticle")]
        [InverseProperty("ArticleTaskDetails")]
        public Article IdArticleNavigation { get; set; }
        [ForeignKey("IdTasks")]
        [InverseProperty("ArticleTaskDetails")]
        public Tasks IdTaskNavigation { get; set; }
    }
}
