using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Plans.PlansData.Activities {
    public class SelectedPlanActivity {

        public int Id { get; set; }

        public int IdActivityVersion { get; set; }

        public int ActivityPosition { get; set; }
        public string WordDescription { get; set; }

        public int SubChapterPosition { get; set; }
        public int ChapterPosition { get; set; }

        public string ChapterDescription { get; set; }
        public string SubChapterDescription { get; set; }

        public int PlanChapterId { get; set; }

        
        /// <summary>
        /// Esto es el id del capitulo...
        /// </summary>
        public int AvailableActivitiId { get; set; }

        public bool IsCustomActivity { get; set; }
        public int CustomActivityId { get; set; }
        public int SubChaptId { get; set; }
    }
}
