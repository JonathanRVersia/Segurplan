using System;
using AutoMapper;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Detail;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList.Dtos;
using Segurplan.Web.Pages.Models.RisksEvaluation.AllocationOfRisksAndPreventiveMeasures.Details;

namespace Segurplan.Web.Pages.Models.RisksEvaluation {
    public class RiskEvaluationProfiles : Profile {
        public RiskEvaluationProfiles() {
            AddFilterProfile();
            AddRiskAndPreventiveMeasureListComponentProfiles();
            AddAllocationOfRisksDetailsProfiles();
        }

        private void AddAllocationOfRisksDetailsProfiles() {
            CreateMap<DetailRiskAndPreventiveMeasuresResponse, RisksAndPreventiveMeasuresDetailModel>();
            CreateMap<PreventiveMeasureDetailDto, AllocationOfRisksAndPreventiveMeasures.Details.PreventiveMeasureModel>()
                .ForMember(dest => dest.PreventiveMeasureCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.PreventiveMeasureDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PreventiveMeasureId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PreventiveMeasureOrder, opt => opt.MapFrom(src => src.Order));

            CreateMap<RisksAndPreventiveMeasuresDetailModel, UpdateRiskAndPreventiveMeasuresModel>();
            CreateMap<AllocationOfRisksAndPreventiveMeasures.Details.PreventiveMeasureModel, UpdatePreventiveMeasure>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PreventiveMeasureId))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.PreventiveMeasureOrder));
        }

        private void AddRiskAndPreventiveMeasureListComponentProfiles() {
            CreateMap<ListRisksAndPreventiveMeasuresResponse.ListItem, RisksAndPreventiveMeasuresTableDataModel>()
                .ForMember(dest => dest.PreventiveMeasure, opt => opt.MapFrom(src => src.PreventiveMeasures))
                .ForMember(dest => dest.TrafficLightsColour, opt => opt.MapFrom(src => src.RiskLevelTrafficLightsColour))
                .ForMember(dest => dest.RiskLevel, opt => opt.MapFrom(src => src.RiskLevelLevel))
                .ReverseMap();
            CreateMap<PreventiveMeasureListDto, Components.RisksAndPreventiveMeasuresList.Dtos.PreventiveMeasureModel>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.PreventiveMeasureDescription));

        }

        private void AddFilterProfile() {
            CreateMap<ChaptSubchaptActIdsFilterModel, ChaptSubChaptActFilterData>();
        }
    }
}


