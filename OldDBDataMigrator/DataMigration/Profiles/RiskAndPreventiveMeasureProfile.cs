using OldDBDataMigrator.ProduccionDBModels;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace OldDBDataMigrator.DataMigration.Profiles {
    public class RiskAndPreventiveMeasureProfile : AutoMapper.Profile {

        public RiskAndPreventiveMeasureProfile() {
            CreateMap<EvaluacionesMedida, RisksAndPreventiveMeasures>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => 1))//Hardcoded
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.FecCreacion))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => 1))//Hardcoded
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.FecModificación == null ? src.FecCreacion : src.FecModificación))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.FecCreacion))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ChapterId, opt => opt.Ignore())
                .ForMember(dest => dest.SubChapterId, opt => opt.Ignore())
                .ForMember(dest => dest.ActivityId, opt => opt.Ignore())
                .ForMember(dest => dest.RiskId, opt => opt.Ignore())
                .ForMember(dest => dest.ProbabilityId, opt => opt.Ignore())
                .ForMember(dest => dest.SeriousnessId, opt => opt.Ignore())
                .ForMember(dest => dest.RiskLevelId, opt => opt.Ignore())
                .ForMember(dest => dest.RiskOrder, opt => opt.Ignore()); 
        }
    }
}
