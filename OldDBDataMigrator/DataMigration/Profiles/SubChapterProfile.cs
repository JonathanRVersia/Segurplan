using System;
using OldDBDataMigrator.ProduccionDBModels;
using OldDBDataMigrator.Utils;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace OldDBDataMigrator.DataMigration.Profiles {
    public class SubChapterProfile :AutoMapper.Profile{
        public SubChapterProfile() {
            CreateMap<Subcapitulos, SubChapter>().ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.Descripcion)))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.FecModificacion))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.SubCapitulo ?? 1))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.FecCreacion));


            CreateMap<Subcapitulos, SubChapterVersion>().ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.SubCapitulo??1))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.FecModificacion))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.FecCreacion))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => DateTime.Now))//HardCoded
                .ForMember(dest => dest.ApprovementDate, opt => opt.MapFrom(src => DateTime.Now))//HardCoded
                .ForMember(dest => dest.IdReviewer, opt => opt.MapFrom(src => 1))//HardCoded
                //En el campo Descripción del Trabajo debe ir el valor del campo Texto
                //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.WorkDetails, opt => opt.MapFrom(src =>src.Descripcion))
                .ForMember(dest => dest.WorkOrganization, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.OrganizacionTrabajo)))
                .ForMember(dest => dest.RiskEvaluation, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.EvaluacionRiesgos)))
                .ForMember(dest => dest.MachineTool, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.MaquinaHerramienta)))
                .ForMember(dest => dest.AssociatedDetails, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.DetallesAsociados)))
                .ForMember(dest => dest.SupportFacilities, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.MediosAuxiliares)));
        }
    }
}
