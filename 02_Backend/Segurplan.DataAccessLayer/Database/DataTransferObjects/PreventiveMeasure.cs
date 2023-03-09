using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class PreventiveMeasure : AuditableTableBase {
        public int Id { get; set; }

        public int Code { get; set; }

        public string Description { get; set; }


        
        public List<RiskAndPreventiveMeasuresMeasures> RisksAndPreventiveMeasures { get; set; }


        [ForeignKey("CreatedBy")]
        [InverseProperty("PreventiveMeasureCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("PreventiveMeasureModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
    }
}
