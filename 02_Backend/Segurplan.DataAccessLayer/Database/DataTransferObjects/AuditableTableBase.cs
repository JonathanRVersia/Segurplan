using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class AuditableTableBase {

        private DateTime creationDate;
        private DateTime updateDate;

        public int CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreateDate {
            get {
                if (creationDate == null || creationDate == DateTime.MinValue) {
                    creationDate = DateTime.Now;
                }

                return creationDate;
            }
            set {
                creationDate = value;
            }
        }

        public int ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdateDate {
            get {
                if (updateDate == null || updateDate == DateTime.MinValue) {
                    updateDate = DateTime.Now;
                }

                return updateDate;
            }
            set {
                updateDate = value;
            }
        }
    }
}
