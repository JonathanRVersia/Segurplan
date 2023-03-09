using System;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Detail;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures {

    public class AllocationOfRisksAndPreventiveMeasuresProfile : AutoMapper.Profile {

        public AllocationOfRisksAndPreventiveMeasuresProfile() {
            AddListProfile();
            AddDetailProfile();
            AddComonDropdownsProfile();
            AddListDropdownsProfile();
            AddDetailsDropdownsProfile();
            AddUpdateProfiles();
        }

        private void AddListProfile() {
            CreateMap<RisksAndPreventiveMeasures, ListRisksAndPreventiveMeasuresResponse.ListItem>()
                .ForMember(dest => dest.RiskLevelValue, opt => opt.MapFrom(src => src.RiskLevel.LevelValue))
                .ForMember(dest => dest.ChapterNumber, opt => opt.MapFrom(src => src.Chapter.Number))
                .ForMember(dest => dest.SubChapterNumber, opt => opt.MapFrom(src => src.SubChapter.Number))
                .ForMember(dest => dest.ActivityNumber, opt => opt.MapFrom(src => src.Activity.Number));
            CreateMap<RiskAndPreventiveMeasuresMeasures, PreventiveMeasureListDto>();
        }

        private void AddDetailProfile() {
            CreateMap<RisksAndPreventiveMeasures, DetailRiskAndPreventiveMeasuresResponse>();

            CreateMap<RiskAndPreventiveMeasuresMeasures, PreventiveMeasureDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PreventiveMeasure.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.PreventiveMeasure.Code))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.PreventiveMeasure.Description))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.PreventiveMeasureOrder));
        }

        private void AddComonDropdownsProfile() {
            CreateMap<Chapter, ChapterDropdownDto>();
            CreateMap<ChapterVersion, ChapterDropdownDto>();
            CreateMap<SubChapter, SubChapterDropdownDto>();
            CreateMap<SubChapterVersion, SubChapterDropdownDto>();
            CreateMap<ChapterVersion, ChapterVersionDto>();
            CreateMap<Activity, ActivityDropdownDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Description));
            CreateMap<Risk, RiskDropdownDto>();
        }

        private void AddListDropdownsProfile() {
            CreateMap<RiskAndPreventiveMeasuresMeasures, PreventiveMeasureDropdownDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PreventiveMeasure.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.PreventiveMeasure.Description));
        }

        private void AddDetailsDropdownsProfile() {
            CreateMap<Probability, ProbabilityDropownsDto>();
            CreateMap<Seriousness, SeriousnessDropdownDto>();
            CreateMap<RiskLevel, RiskLevelDropdownDto>();
            CreateMap<RiskLevelBySeriousnessAndProbability, RiskLevelBySeriousnessAndProbabilitiesDto>();
        }

        private void AddUpdateProfiles() {
            CreateMap<UpdateRiskAndPreventiveMeasuresModel, RisksAndPreventiveMeasures>();
            CreateMap<UpdatePreventiveMeasure, PreventiveMeasure>();
            CreateMap<UpdatePreventiveMeasure, RiskAndPreventiveMeasuresMeasures>();
        }

    }
}
