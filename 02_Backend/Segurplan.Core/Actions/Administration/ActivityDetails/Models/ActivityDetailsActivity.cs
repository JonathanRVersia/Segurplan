using System.Collections.Generic;
using Segurplan.Core.Actions.Administration.SubChapterDetails.Models;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Models {
    public class ActivityDetailsActivity {

        public int Id { get; set; }

        public int SubChapterId { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public string WordDescription { get; set; }

        //public User CreatedByNavigation { get; set; }

        //public User ModifiedByNavigation { get; set; }

        public SubChapterDetailsSubChapter SubChapter { get; set; }

        public ICollection<ActivityDetailsActivityVersion> ActivityVersion { get; set; }
    }
}
