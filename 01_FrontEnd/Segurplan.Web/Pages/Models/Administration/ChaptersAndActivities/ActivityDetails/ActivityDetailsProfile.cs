
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;
using Segurplan.Core.Actions.Administration.ActivityDetails.Save;

namespace Segurplan.Web.Pages.Models.Administration.ChaptersAndActivities.ActivityDetails {
    public class ActivityDetailsProfile : AutoMapper.Profile {
        public ActivityDetailsProfile() {
            CreateMap<ActivityDetailsActivityVersion, ActivityDetailsViewModel>();
            CreateMap<ActivityDetailsViewModel, SaveActivityRequest>()
                .ForMember(dest=>dest.SubChapterId,opt=>opt.MapFrom(src=>src.IdActivityNavigation.SubChapterId));
        }
    }
}
