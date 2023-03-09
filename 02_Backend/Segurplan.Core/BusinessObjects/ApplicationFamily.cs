using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationFamily {
        public int Id { get; set; }
        public string Family { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
