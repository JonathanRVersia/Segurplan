using OldDBDataMigrator.ProduccionDBModels;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace OldDBDataMigrator.DataMigration.Profiles {
    public class PreventiveMeasuresProfile : AutoMapper.Profile {
        public PreventiveMeasuresProfile() {

            CreateMap<Medidas, PreventiveMeasure>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.FecModificacion))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.FecCreacion))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => 1))//Hardcoded
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => 1))//Hardcoded
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Medida));
        }
    }
}
