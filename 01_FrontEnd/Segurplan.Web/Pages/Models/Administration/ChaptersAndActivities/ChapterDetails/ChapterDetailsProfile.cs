using Segurplan.Core.Actions.Administration.ChapterDetails.Models;
using Segurplan.Core.Actions.Administration.ChapterDetails.Save;

namespace Segurplan.Web.Pages.Models.Administration.ChaptersAndActivities.ChapterDetails {
    public class ChapterDetailsProfile :AutoMapper.Profile {
        public ChapterDetailsProfile() {
            CreateMap<ChapterDetailsChapterVersion, ChapterDetailsViewModel>();
            CreateMap<ChapterDetailsViewModel, SaveChapterRequest>();
        }
    }
}
