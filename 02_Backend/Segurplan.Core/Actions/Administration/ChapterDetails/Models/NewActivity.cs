using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Models {
    public class NewActivity {
        public int Id { get; set; }

        public int SubChapterId { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public string WordDescription { get; set; }

        public NewSubChapter SubChapter { get; set; }

        public ICollection<NewActivityVersion> ActivityVersion { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int CreatedBy { get; set; }

        public int ModifiedBy { get; set; }
    }
}
