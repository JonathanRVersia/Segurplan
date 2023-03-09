using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.BusinessObjects;
using Segurplan.Web.Pages.Components.PlanActivityCustomModals;

namespace Segurplan.Web.Pages.Models.SafetyPlans {
    public class PlanManagementModelProfile : AutoMapper.Profile {

        public PlanManagementModelProfile() {
            MapGetCustomPlanChapters();
            MapPlanActivityCustomModalsModels();
        }

        /// <summary>
        /// Used on GetCustomPlanChapterRequestHandler
        /// </summary>
        private void MapGetCustomPlanChapters() {
            CreateMap<CustomPlanChapter, PlanChapter>()
                .ForMember(dest => dest.SubChapter, opt => opt.MapFrom(src => src.Subchapters))
                .ForMember(dest => dest.IsCustomChapter, opt => opt.MapFrom(src => true));
            CreateMap<CustomPlanSubchapter, PlanSubChapter>()
                .ForMember(dest => dest.IsCustomSubChapter, opt => opt.MapFrom(src => true));
            CreateMap<CustomPlanActivity, PlanActivity>()
                .ForMember(dest => dest.IsCustomActivity, opt => opt.MapFrom(src => true));
        }

        /// <summary>
        /// Used for the communication between PlanActivityCustomModalsViewComponent and PlanManagement.cs in both directions
        /// </summary>
        private void MapPlanActivityCustomModalsModels() {
            CreateMap<ViewComponentCustomPlanChapter, PlanChapter>();
            CreateMap<PlanSubChapter, ViewComponentCustomPlanSubChapter>();
            CreateMap<PlanActivity, ViewComponentCustomPlanActivity>();

            CreateMap<PlanChapter, ViewComponentCustomPlanChapter>();
            CreateMap<ViewComponentCustomPlanSubChapter, PlanSubChapter>();
            CreateMap<ViewComponentCustomPlanActivity, PlanActivity>();
                //.ForMember(dest=>dest.Title,opt=>opt.MapFrom(src=>src.Description));
        }
    }
}
