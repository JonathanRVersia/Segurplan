using System;

namespace Segurplan.Core.Database.DataTransferObjects {
    public class SegurplanDTOBase {
        public int Id { get; set; }

        public UserDTO CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public UserDTO UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
