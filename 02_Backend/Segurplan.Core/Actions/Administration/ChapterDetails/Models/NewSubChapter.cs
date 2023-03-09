using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Models {
    public class NewSubChapter {
        public int Id { get; set; }

        public int IdChapter { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<NewActivity> Activity { get; set; }

        public ICollection<NewSubChapterVersion> SubChapterVersion { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int CreatedBy { get; set; }

        public int ModifiedBy { get; set; }
    }
}
