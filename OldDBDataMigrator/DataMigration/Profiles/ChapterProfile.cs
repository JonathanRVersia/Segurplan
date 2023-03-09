using System;
using OldDBDataMigrator.ProduccionDBModels;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using OldDBDataMigrator.Utils;

namespace OldDBDataMigrator.DataMigration.Profiles {
    public class ChapterProfile : AutoMapper.Profile{
        public ChapterProfile() {

            CreateMap<Capitulos, Chapter>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src=>src.Titulo))
                .ForMember(dest => dest.WordDescription, opt => opt.MapFrom(src => RichTextCleanerForAutomapper.CleanRichText(src.DescripcionWord)))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.FecModificacion))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src=>src.FecCreacion))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Capitulo ?? 1))
                .ForMember(dest => dest.DefaultSelectedChapter, opt => opt.MapFrom(src=>false));

            CreateMap<Capitulos, ChapterVersion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Capitulo??1))
                .ForMember(dest => dest.VersionNumber, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => 1))//HardCoded
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.FecModificacion))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.FecCreacion))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.FecRevisionAnt))
                .ForMember(dest => dest.ApprovementDate, opt => opt.MapFrom(src => src.FecRevision))
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
