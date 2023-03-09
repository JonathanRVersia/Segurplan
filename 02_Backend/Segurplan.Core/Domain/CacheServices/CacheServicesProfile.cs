using System.Linq;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Domain.CacheServices {
    public class CacheServicesProfile : AutoMapper.Profile {
        public CacheServicesProfile() {
            CreateMap<Chapter, PlanChapter>();
            CreateMap<SubChapter, PlanSubChapter>()
                .ForMember(dest=>dest.Activities,opt=>opt.MapFrom(src=>src.Activity));
            CreateMap<Activity, PlanActivity>();
            CreateMap<ChapterVersion, PlanChapter>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdChapter))
                .ForMember(dest => dest.DefaultSelectedChapter, opt => opt.MapFrom(src => src.IdChapterNavigation.DefaultSelectedChapter))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.IdChapterNavigation.Number))
                .ForMember(dest => dest.SubChapter, opt => opt.MapFrom(src => src.SubChapterVersion.Select(x=>x.IdSubChapterNavigation)))
                ;
        }
    }
}
