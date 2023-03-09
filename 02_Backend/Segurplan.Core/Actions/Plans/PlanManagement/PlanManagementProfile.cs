using Segurplan.Core.Actions.AllDocuments.Models;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.Core.Actions.Plans.PlanManagement.Dropdowns.UpdateAffiliatedDependantData;
using System;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Actions.Plans.PlanManagement.DefaultValues;

namespace Segurplan.Core.Actions.Plans.PlanManagement {
    public class PlanManagementProfile : AutoMapper.Profile {
        public PlanManagementProfile() {
            AddGenerateProfiles();
            UpdateAffiliatedDependantDataProfiles();
            AddDownloadProfiles();
            CreateProfiles();
            PlanDetailsDefaultValuesProfiles();
        }

        private void PlanDetailsDefaultValuesProfiles() {
            CreateMap<PlanDetailsDefaultValues, PlanDetailsDefaultValuesDto>();


        }

        private void CreateProfiles() {
            CreateMap<DefaultSafetyStudyPlanFile, PlanFile>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.DataLength, opt => opt.MapFrom(src => src.FileSize));
        }

        private void AddDownloadProfiles() {
            CreateMap<DefaultSafetyStudyPlanFile, SafetyStudyPlanFile>();
        }

        private void AddGenerateProfiles() {
            CreateMap<SafetyStudyPlan, DocumentContent>();
            CreateMap<PlanChapter, CustomChapter>()
                .ForMember(dest => dest.CustomSubchapters, opt => opt.MapFrom(src => src.SubChapter));
            CreateMap<PlanSubChapter, CustomSubchapter>()
                .ForMember(dest => dest.CustomActivities, opt => opt.MapFrom(src => src.Activities));
            CreateMap<PlanActivity, CustomActivity>();
            CreateMap<PlanChapter, ChapterVersion>()
                .ForMember(dest => dest.SubChapterVersion, opt => opt.MapFrom(src => src.SubChapter))
                .ForMember(dest=>dest.WordDescription,opt=>opt.MapFrom(src=>src.WordDescription));
            CreateMap<PlanSubChapter, SubChapterVersion>()
                .ForMember(dest => dest.ActivityVersion, opt => opt.MapFrom(src => src.Activities))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.WordDescription));
            CreateMap<PlanActivity, ActivityVersion>()
                .ForMember(dest => dest.WordDescription, opt => opt.MapFrom(src => src.WordDescription));
        }

        private void UpdateAffiliatedDependantDataProfiles() {
            CreateMap<DataAccessLayer.Database.DataTransferObjects.Delegation, SelectDataDto>();
            CreateMap<BusinessAddress, SelectDataDto>();
        }
    }
}
