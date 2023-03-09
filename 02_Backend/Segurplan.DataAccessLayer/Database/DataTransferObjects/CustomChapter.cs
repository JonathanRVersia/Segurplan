using System.Collections.Generic;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class CustomChapter : AuditableTableBase {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<CustomSubchapter> CustomSubchapters { get; set; }
    }
}
