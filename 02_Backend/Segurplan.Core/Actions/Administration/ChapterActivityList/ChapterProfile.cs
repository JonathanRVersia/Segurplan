using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.Core.Actions.Administration.ChapterActivityList {
    public class ChapterProfile : AutoMapper.Profile{
        public ChapterProfile() {
            CreateMap<Chapter, ChapterActivityListResponse.ListItem>();
            CreateMap<ChapterVersion, ChapterActivityListResponse.ChapterActivityListChapterVersion>();
            CreateMap<User, ChapterActivityListResponse.ChapterActivityListUser>();
            //CreateMap<ChapterVersionInfo, ChapterActivityListResponse.ChapterActivityListVersionInfo>();
        }
    }
}
