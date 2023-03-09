using Segurplan.Core.Actions.Administration.SubChapterDetails.Models;
using Segurplan.Core.Actions.Administration.SubChapterDetails.Save;

namespace Segurplan.Web.Pages.Models.Administration.ChaptersAndActivities.SubChapterDetails {
    public class SubChapterDetailsProfile :AutoMapper.Profile {
        public SubChapterDetailsProfile() {
            CreateMap<SubChapterDetailsSubChapterVersion, SubChapterDetailsViewModel>()
                .ForMember(src=>src.ChapterId,opt=>opt.MapFrom(dest=>dest.IdSubChapterNavigation.IdChapterNavigation.Id));
            CreateMap<SubChapterDetailsViewModel, SaveSubChapterRequest>();
        }
    }
}
