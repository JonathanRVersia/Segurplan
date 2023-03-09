
namespace Segurplan.Core.Actions.Administration.ActivityDetails.Models {
    public class ActivityDetailsSubChapterVersion {
        public int Id { get; set; }

        public string Description { get; set; }

        public ActivityDetailsChapterVersion IdChapterVersionNavigation { get; set; }
    }
}
