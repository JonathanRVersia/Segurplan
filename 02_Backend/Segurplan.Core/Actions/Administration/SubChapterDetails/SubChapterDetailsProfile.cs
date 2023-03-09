using Segurplan.Core.Actions.Administration.SubChapterDetails.Models;
using Segurplan.Core.Actions.Administration.SubChapterDetails.Save;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails {
    public class SubChapterDetailsProfile :AutoMapper.Profile{
        public SubChapterDetailsProfile() {
            CreateMap<SubChapter, SubChapterDetailsSubChapter>();
            CreateMap<SubChapterVersion, SubChapterDetailsSubChapterVersion>();
            CreateMap<Chapter, SubChapterDetailsChapter>();
            //CreateMap<Activity, SubChapterDetailsActivity>();
            CreateMap<ActivityVersion, SubChapterDetailsActivity>();
            CreateMap<SaveSubChapterRequest, SubChapterVersion>();
        }
    }
}
