using System.Collections.Generic;

namespace Segurplan.Core.Actions.Plans.PlansData.Activities {
    public class PlanSubChapter {
        public int Id { get; set; }

        public int Position { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string WordDescription { get; set; }

        public bool IsSelected { get; set; }

        public List<PlanActivity> Activities { get; set; }

        public int ChapterPosition { get; set; }

        public bool IsCustomSubChapter { get; set; }

        public int ChapterId { get; set; }

        public PlanSubChapter() {

        }

        public PlanSubChapter(PlanSubChapter nuevo) {

            this.Id = nuevo.Id;
            this.Number = nuevo.Number;
            this.Title = nuevo.Title;
            this.Description = nuevo.Description;
            this.Activities = new List<PlanActivity>();
        }
    }
}
