using System.ComponentModel.DataAnnotations.Schema;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class PlanActivityVersion {

        public int Id { get; set; }

        public int? IdActivityVersion { get; set; }

        public int? CustomActivityId { get; set; }

        public int IdPlan { get; set; }

        public int Position { get; set; }

        public string WordDescription { get; set; }

        public int SubChapterPosition { get; set; }

        public int ChapterPosition { get; set; }

        public string ChapterDescription { get; set; }

        public string SubChapterDescription { get; set; }
        public int AvailableActivitiId { get; set; }


        [ForeignKey("IdPlan")]
        [InverseProperty("PlanActivityVersion")]
        public SafetyStudyPlan IdPlanNavigation { get; set; }

        [ForeignKey("IdActivityVersion")]
        [InverseProperty("PlanActivityVersion")]
        public ActivityVersion IdActivityVersionNavigation { get; set; }

        public CustomActivity CustomActivity { get; set; }
        public int SubChaptId { get; set; }
    }
}
