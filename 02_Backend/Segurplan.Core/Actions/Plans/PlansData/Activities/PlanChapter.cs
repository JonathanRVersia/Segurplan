using System.Collections.Generic;

namespace Segurplan.Core.Actions.Plans.PlansData.Activities {
    public class PlanChapter {       

        public int Id { get; set; }
        
        public int Position { get; set; }

        public int Number { get; set; }
        
        public string Title { get; set; }

        public string WordDescription { get; set; }

        public bool IsSelected { get; set; }

        public List<PlanSubChapter> SubChapter { get; set; }
        public bool DefaultSelectedChapter { get; set; }

        public bool IsCustomChapter { get; set; }
    }
}
