using System;
using OldDBDataMigrator.ProduccionDBModels;
using OldDBDataMigrator.Utils;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace OldDBDataMigrator.DataMigration.Profiles {
    public class ActivityProfile : AutoMapper.Profile {

        public ActivityProfile() {

            CreateMap<Actividades, Activity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.WordDescription, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.DescripcionWord)))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.FecModificacion))
                 .ForMember(dest => dest.Number, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Actividad) ? int.Parse(src.Actividad) : 1))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.FecCreacion));


            CreateMap<Actividades, ActivityVersion>().ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Actividad) ? int.Parse(src.Actividad) : 1))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.FecModificacion))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.FecCreacion))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => DateTime.Now))//HardCoded
                .ForMember(dest => dest.ApprovementDate, opt => opt.MapFrom(src => DateTime.Now))//HardCoded
                .ForMember(dest => dest.IdReviewer, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.WordDescription, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.DescripcionWord)))
                .ForMember(dest => dest.WorkDetails, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.DescripcionTrabajo)))
                .ForMember(dest => dest.WorkOrganization, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.OrganizacionTrabajo)))
                .ForMember(dest => dest.RiskEvaluation, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.EvaluacionRiesgos)))
                .ForMember(dest => dest.MachineTool, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.MaquinaHerramienta)))
                .ForMember(dest => dest.AssociatedDetails, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.DetallesAsociados)))
                .ForMember(dest => dest.SupportFacilities, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.MediosAuxiliares)));
        }
    }
}
