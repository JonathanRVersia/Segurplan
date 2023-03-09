
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Administration.Risks {
    public class RiskProfile : AutoMapper.Profile{
        public RiskProfile() {
            CreateMap<Risk, ApplicationRisk>().ReverseMap();
        }
    }
}
