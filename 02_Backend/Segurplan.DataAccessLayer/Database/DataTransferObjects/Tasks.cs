using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class Tasks: AuditableTableBase {
        public Tasks() {
            this.ArticleTaskDetails = new HashSet<ArticleTaskDetail>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("TaskCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("TaskModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        [InverseProperty("IdTaskNavigation")]
        public ICollection<ArticleTaskDetail> ArticleTaskDetails { get; set; }
    }
}
