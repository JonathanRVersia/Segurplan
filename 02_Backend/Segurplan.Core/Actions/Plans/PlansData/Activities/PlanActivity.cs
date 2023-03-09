namespace Segurplan.Core.Actions.Plans.PlansData.Activities {
    public class PlanActivity {

        public int Id { get; set; }

        public int Position { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public string WordDescription { get; set; }

        public bool IsSelected { get; set; }

        public int SubChapterPosition { get; set; }

        public int ChapterPosition { get; set; }

        public bool IsCustomActivity { get; set; }

        //Used when creates new Activity for a Plan
        //public string Title { get; set; }

        public int SubChapterId { get; set; }


        public PlanActivity() {

        }

        public PlanActivity(PlanActivity nuevo) {

            this.Id = nuevo.Id;
            this.Number = nuevo.Number;
            this.Description = nuevo.Description;
        }
    }
}
