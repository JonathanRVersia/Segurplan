using Segurplan.Core.Actions.Administration.ActivityDetails.Models;
using Segurplan.Core.Actions.Administration.ActivityDetails.Save;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Administration.ActivityDetails {
    public class ActivityDetailsProfile : AutoMapper.Profile {
        public ActivityDetailsProfile() {
            CreateMap<ActivityVersion, ActivityDetailsActivityVersion>();
            CreateMap<Activity, ActivityDetailsActivity>();
            CreateMap<ChapterVersion, ActivityDetailsChapterVersion>();
            CreateMap<SubChapterVersion, ActivityDetailsSubChapterVersion>();
            CreateMap<SaveActivityRequest, ActivityVersion>();
            CreateMap<ActivityRelations,ActivityRelationsModel>().ReverseMap();
        }
    }
}
