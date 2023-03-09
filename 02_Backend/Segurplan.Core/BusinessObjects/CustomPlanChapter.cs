using System.Collections.Generic;

namespace Segurplan.Core.BusinessObjects {
    public class CustomPlanChapter {
        //public string Name { get; set; }
        public int Id { get; set; }

        public string Title { get; set; }

        public string WordDescription { get; set; }

        public int Position { get; set; }

        public int Number { get; set; }

        public List<CustomPlanSubchapter> Subchapters { get; set; }
    }
}
