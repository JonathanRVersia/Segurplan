using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Segurplan.Core.Actions.Administration.Seriousness.Save;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationSeriousness {
        public ApplicationSeriousness() {
        }

        public ApplicationSeriousness(int id, string value) {
            Id = id;
            Value = value;
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public List<TableMatrixValues> TableMatrixValues { get; set; }
    }
}
