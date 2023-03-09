
using System.Collections.Generic;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class CustomSubchapter : AuditableTableBase {

        public int Id { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public CustomChapter CustomChapter { get; set; }

        public int? CustomChapterId { get; set; }

        public List<CustomActivity> CustomActivities { get; set; }

        public Chapter Chapter { get; set; }

        public int? ChapterId { get; set; }
    }
}
