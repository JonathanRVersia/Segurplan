using Segurplan.Core.Actions.Administration.Articles.ModalList;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read {
    public class ReadProfile : AutoMapper.Profile {

        public ReadProfile() {
            AddViewPlanPlanesProfiles();
        }

        private void AddViewPlanPlanesProfiles() {
            CreateMap<SafetyStudyPlanPlane, SafetyPlanPlane>()
                .ForMember(dest => dest.IdPlan, opt => opt.MapFrom(src => src.IdSafetyStudyPlan));

            //general planes
            CreateMap<PlaneFamily, ApplicationPlaneFamily>()
                .ForMember(dest => dest.FamilyId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.Family))
                .ForMember(dest => dest.PlaneList, opt => opt.MapFrom(src => src.Plane));
            //CreateMap<ICollection<Plane>, List<ApplicationPlane>>();
            CreateMap<Plane, ApplicationPlane>()
                .ForMember(dest => dest.HasFile, opt => opt.MapFrom(src => src.Data != null ? true : false));

            //general presupuesto
            CreateMap<ArticleFamily, ApplicationArticleFamily>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Family, opt => opt.MapFrom(src => src.Family))
                .ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.Article));
            //CreateMap<, >();

            CreateMap<Tasks, ApplicationTask>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TaskDetails, opt => opt.MapFrom(src => src.ArticleTaskDetails));

            CreateMap<ApplicationTask, Tasks>();

            CreateMap<Article, ApplicationArticle>();
            CreateMap<ApplicationArticle, Article>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.Percentage))
                .ForMember(dest => dest.TimeOfWork, opt => opt.MapFrom(src => src.TimeOfWork))
                .ForMember(dest => dest.MinimumUnit, opt => opt.MapFrom(src => src.MinimumUnit))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.AmortizationTime, opt => opt.MapFrom(src => src.AmortizationTime));
                
            CreateMap<ArticleTaskDetail, ApplicationArticleTaskDetail>()
                .ForMember(dest => dest.Article, opt => opt.MapFrom(src => src.IdArticleNavigation));
            CreateMap<ApplicationArticleTaskDetail, ArticleTaskDetail>();

            CreateMap<Article, ArticlesListResponse.ListItem>();

        }
    }
}
