using System;
using OldDBDataMigrator.ProduccionDBModels;
using Segurplan.DataAccessLayer.Database.Identity;

namespace OldDBDataMigrator.DataMigration.Profiles {
    public class UsersProfile : AutoMapper.Profile {
        public UsersProfile() {
            CreateMap<Usuarios, User>()
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Usuario.ToUpper()))
                .ForMember(dest => dest.SecurityStamp, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CompleteName, opt => opt.MapFrom(src => src.Nombre));
        }
    }
}
