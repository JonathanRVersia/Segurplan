using System;
using System.Collections.Generic;

namespace Segurplan.Core.Database.Models {
    public partial class SelectionType {
        public SelectionType() {
            Template = new HashSet<Template>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<Template> Template { get; set; }
    }
}
