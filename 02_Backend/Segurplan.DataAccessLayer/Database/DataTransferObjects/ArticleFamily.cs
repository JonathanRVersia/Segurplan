using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class ArticleFamily: AuditableTableBase {
        public int Id { get; set; }
        [Required]
        public string Family { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("ArticleFamilyCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("ArticleFamilyModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        [InverseProperty("IdArticleFamilyNavigation")]
        public ICollection<Article> Article { get; set; }
    }
}
