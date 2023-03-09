using System;
using Segurplan.Core.Actions.Administration.ChapterDetails.Models;
using Segurplan.Core.Actions.Administration.ChapterDetails.Save;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.Core.Actions.Administration.ChapterDetails {
    public class ChapterVersionProfile : AutoMapper.Profile {
        public ChapterVersionProfile() {
            CreateMap<ChapterVersion, ChapterDetailsChapterVersion>();
            CreateMap<SubChapterVersion, ChapterDetailsSubChapterVersion>();
            CreateMap<User, ChapterDetailsUserInfo>();
            //CreateMap<ChapterVersionInfo, ChapterDetailsVersionInfo>();
            CreateMap<Chapter, ChapterDetailsChapter>();
            CreateMap<ChapterVersion, OtherChapterVersions>();
            CreateMap<UserChapterVersion, UserChapterDetails>();
            CreateMap<SaveChapterRequest, ChapterVersion>()
                .ForMember(dest => dest.Number, opt => opt.Ignore());
            CreateMap<UserChapterDetails, UserChapterVersion>();

            CreateMap<ChapterVersion, NewChapterVersion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ApprovementDate, opt => opt.Ignore())
                .ForMember(dest => dest.EndDate, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewDate, opt => opt.Ignore())
                .ForMember(dest => dest.VersionNumber, opt => opt.MapFrom(src => src.VersionNumber + 1))
                ;
            CreateMap<SubChapterVersion, NewSubChapterVersion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IdSubChapter, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.Now))
                ;
            CreateMap<ActivityVersion, NewActivityVersion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IdActivity, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.Now))
                ;

            CreateMap<SubChapter, NewSubChapter>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.Now))
                ;
            CreateMap<Activity, NewActivity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SubChapterId, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.Now))
                ;

            CreateMap<NewChapterVersion, ChapterVersion>();
            CreateMap<NewSubChapterVersion, SubChapterVersion>();
            CreateMap<NewActivityVersion, ActivityVersion>();
            CreateMap<NewSubChapter, SubChapter>();
            CreateMap<NewActivity, Activity>();

        }
    }
}
