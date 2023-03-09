
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Administration.Family {
    public class FamilyProfile :AutoMapper.Profile{
        public FamilyProfile() {
            CreateMap<ArticleFamily, ApplicationFamily>().ReverseMap();
        }
    }
}
