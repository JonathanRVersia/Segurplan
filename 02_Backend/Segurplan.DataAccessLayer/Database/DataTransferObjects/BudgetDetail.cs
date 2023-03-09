using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class BudgetDetail: AuditableTableBase {
        public int Id { get; set; }
        [Required]
        public int QuantityUnits { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int IdArticle { get; set; }
        [Required]
        public int IdBudget { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("BudgetDetailCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("BudgetDetailModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        [ForeignKey("IdArticle")]
        [InverseProperty("BudgetDetails")]
        public Article IdArticleNavigation { get; set; }
        [ForeignKey("IdBudget")]
        [InverseProperty("BudgetDetails")]
        public Budget IdBudgetNavigation { get; set; }
    }
}
