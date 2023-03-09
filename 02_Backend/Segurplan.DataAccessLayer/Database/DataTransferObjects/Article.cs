using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class Article : AuditableTableBase {
        public Article() {
            this.ArticleTaskDetails = new HashSet<ArticleTaskDetail>();
            this.BudgetDetails = new HashSet<BudgetDetail>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Percentage { get; set; }
        public decimal TimeOfWork { get; set; }
        [Required]
        public int MinimumUnit { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal AmortizationTime { get; set; }

        public int IdArticleFamily { get; set; }
        [ForeignKey("IdArticleFamily")]
        [InverseProperty("Article")]
        public ArticleFamily IdArticleFamilyNavigation { get; set; }
        
        [ForeignKey("CreatedBy")]
        [InverseProperty("ArticleCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        
        [ForeignKey("ModifiedBy")]
        [InverseProperty("ArticleModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        
        [InverseProperty("IdArticleNavigation")]
        public ICollection<ArticleTaskDetail> ArticleTaskDetails { get; set; }
        [InverseProperty("IdArticleNavigation")]
        public ICollection<BudgetDetail> BudgetDetails { get; set; }
    }
}
