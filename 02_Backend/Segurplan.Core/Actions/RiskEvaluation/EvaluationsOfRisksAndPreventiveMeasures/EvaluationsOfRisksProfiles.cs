using Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.RiskEvaluation.EvaluationsOfRisksAndPreventiveMeasures {
    public class EvaluationsOfRisksProfiles : AutoMapper.Profile {

        public EvaluationsOfRisksProfiles() {
            AddGenerateProfiles();

        }

        private void AddGenerateProfiles() {
            CreateMap<ListRisksAndPreventiveMeasuresResponse.ListItem, RiskAndPreventiveMeasuresDocumentDto>();
            CreateMap<PreventiveMeasureListDto, PreventiveMeasureListDocumentDto>()
                .ForMember(dest => dest.PreventiveMeasureDescriptionHtml, opt => opt.MapFrom(src => src.PreventiveMeasureDescription));
            CreateMap<Chapter, PlanChapterDocumentDto>()
                .ForMember(dest => dest.SubChaptersHtml, opt => opt.MapFrom(src => src.SubChapter));
            CreateMap<SubChapter, PlanSubChapterDocumentDto>()
                .ForMember(dest => dest.ActivitiesHtml, opt => opt.MapFrom(src => src.Activity));
            CreateMap<Activity, PlanActivityDocumentDto>();
        }
    }
}
