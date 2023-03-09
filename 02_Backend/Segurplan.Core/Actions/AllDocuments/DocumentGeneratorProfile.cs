using Segurplan.Core.Actions.AllDocuments.Models;
using Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos;
using Segurplan.Core.Actions.Plans.PlanManagement.Files.View;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.AllDocuments {
    public class DocumentGeneratorProfile : AutoMapper.Profile {

        public DocumentGeneratorProfile() {

            AddBlueprintProfiles();
            AddChapterHierarchyProfiles();
            AddDocumentContentProfiles();
            AddPreventiveMeasuresProfile();
        }

        private void AddPreventiveMeasuresProfile() {
            CreateMap<RisksAndPreventiveMeasures, RiskAndPreventiveMeasuresDocumentDto>();

            CreateMap<RiskAndPreventiveMeasuresMeasures, PreventiveMeasureListDocumentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PreventiveMeasure.Id))
                .ForMember(dest => dest.PreventiveMeasureDescriptionHtml, opt => opt.MapFrom(src => src.PreventiveMeasure.Description));


            CreateMap<RiskAndPreventiveMeasuresDocumentDto, PlanFormatoSinTablasActivityRisks>();

            CreateMap<RiskAndPreventiveMeasuresDocumentDto, MeasuresPerRiskAndActivity>()
                 .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(src => src.ActivityDescription))
                 .ForMember(dest => dest.PreventiveMeasuresHtml, opt => opt.MapFrom(src => src.PreventiveMeasures));
        }

        private void AddDocumentContentProfiles() {
            CreateMap<RiskAndPreventiveMeasuresMeasures, DocumentContent>();

            CreateMap<SafetyPlan, DocumentContent>()
                .ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.GeneralData.PlanTitle))
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.GeneralData.CreatorName))
                .ForMember(dest => dest.ApproverName, opt => opt.MapFrom(src => src.GeneralData.ApproverName))
                .ForMember(dest => dest.WorkLocation, opt => opt.MapFrom(src => src.AdditionalData.Localization))
                .ForMember(dest => dest.Municipality, opt => opt.MapFrom(src => src.AdditionalData.Municipality))
                .ForMember(dest => dest.ExecutionTimeMonths, opt => opt.MapFrom(src => src.AdditionalData.ExecutionTimeMonths))
                .ForMember(dest => dest.ExecutionBudget, opt => opt.MapFrom(src => src.AdditionalData.ExecutionBudget))
                .ForMember(dest => dest.PSSBudget, opt => opt.MapFrom(src => src.AdditionalData.PSSBudget))
                .ForMember(dest => dest.WorkersNumber, opt => opt.MapFrom(src => src.AdditionalData.WorkersNumber))
                .ForMember(dest => dest.WorkDescriptionHtml, opt => opt.MapFrom(src => src.AdditionalData.SituationDescription ?? string.Empty))
                .ForMember(dest => dest.AffectedServicesDescriptionHtml, opt => opt.MapFrom(src => src.AdditionalData.AffectedServicesDescription ?? string.Empty))
                .ForMember(dest => dest.OrganizationalStructureHtml, opt => opt.MapFrom(src => src.AdditionalData.OrganizationalStructure ?? string.Empty))
                .ForMember(dest => dest.AssistanceCentersHtml, opt => opt.MapFrom(src => src.AdditionalData.AssistanceCenters ?? string.Empty))
                .ForMember(dest => dest.EmergencyPlanDescriptionHtml, opt => opt.MapFrom(src => src.AdditionalData.EmergencyPlanDescription ?? string.Empty))
                .ForMember(dest => dest.ActivityDescriptionHtml, opt => opt.MapFrom(src => src.AdditionalData.ActivityDescription ?? string.Empty));
        }

        private void AddChapterHierarchyProfiles() {
            CreateMap<PlanChapter, PlanChapterDocumentDto>();
            CreateMap<PlanSubChapter, PlanSubChapterDocumentDto>();
            CreateMap<PlanActivity, PlanActivityDocumentDto>();

            CreateMap<ChapterVersion, PlanChapterDocumentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdChapter))
                .ForMember(dest => dest.SubChaptersHtml, opt => opt.MapFrom(src => src.SubChapterVersion))
                .ForMember(dest => dest.WorkDetailsHtml, opt => opt.MapFrom(src => src.WorkDetails ?? string.Empty))
                .ForMember(dest => dest.WordDescriptionHtml, opt => opt.MapFrom(src => src.WordDescription ?? string.Empty))
                .ForMember(dest => dest.MachineToolHtml, opt => opt.MapFrom(src => src.MachineTool ?? string.Empty))
                .ForMember(dest => dest.WorkOrganizationHtml, opt => opt.MapFrom(src => src.WorkOrganization ?? string.Empty));

            CreateMap<SubChapterVersion, PlanSubChapterDocumentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdSubChapter))
                .ForMember(dest => dest.ActivitiesHtml, opt => opt.MapFrom(src => src.ActivityVersion))
                .ForMember(dest => dest.WorkDetailsHtml, opt => opt.MapFrom(src => src.WorkDetails ?? string.Empty))
                .ForMember(dest => dest.WordDescriptionHtml, opt => opt.MapFrom(src => src.Description ?? string.Empty))
                .ForMember(dest => dest.WorkOrganizationHtml, opt => opt.MapFrom(src => src.WorkOrganization ?? string.Empty))
                .ForMember(dest => dest.MachineToolHtml, opt => opt.MapFrom(src => src.MachineTool ?? string.Empty));

            CreateMap<ActivityVersion, PlanActivityDocumentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdActivity))
                .ForMember(dest => dest.WorkDetailsHtml, opt => opt.MapFrom(src => src.WorkDetails ?? string.Empty))
                .ForMember(dest => dest.WordDescriptionHtml, opt => opt.MapFrom(src => src.WordDescription ?? string.Empty))
                .ForMember(dest => dest.MachineToolHtml, opt => opt.MapFrom(src => src.MachineTool ?? string.Empty))
                .ForMember(dest => dest.WorkOrganizationHtml, opt => opt.MapFrom(src => src.WorkOrganization ?? string.Empty));
        }

        private void AddBlueprintProfiles() {
            CreateMap<SafetyPlanPlane, SafetyPlanPlaneDocumentDto>();
            CreateMap<ViewPlanPlaneItem, ViewPlanPlaneItemDocumentDto>();
        }
    }
}
