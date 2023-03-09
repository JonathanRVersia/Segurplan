using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Models {
    public class SubChapterDetailsSubChapter {

        public int Id { get; set; }

        //public int Number { get; set; }
        public SubChapterDetailsChapter IdChapterNavigation { get; set; }

        public string Title { get; set; }

        public int Number { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
