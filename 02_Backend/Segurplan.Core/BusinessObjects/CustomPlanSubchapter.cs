using System.Collections.Generic;

namespace Segurplan.Core.BusinessObjects {
    public class CustomPlanSubchapter {
        //public string Name { get; set; }
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string WordDescription { get; set; }

        public int Position { get; set; }

        public int ChapterPosition { get; set; }

        public int Number { get; set; }

        public List<CustomPlanActivity> Activities { get; set; }

        public int CustomChapterId { get; set; }

        public int? ChapterId { get; set; }
    }
}
