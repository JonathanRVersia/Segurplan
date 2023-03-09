using System.Collections.Generic;
using Segurplan.Core.Actions.Administration.Seriousness.MatrixDataList;
using Segurplan.Core.Actions.Administration.Seriousness.Save;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Administration.Seriousness {
    public class SeriousnessProfile : AutoMapper.Profile {
        public SeriousnessProfile() {
            MapMatrixModels();
            AddSaveMaps();
            CreateMap<DataAccessLayer.Database.DataTransferObjects.Seriousness, ModalList.SeriousnessListResponse.ListItem>();
        }

        private void AddSaveMaps() {
            CreateMap<DataAccessLayer.Database.DataTransferObjects.Seriousness, ApplicationSeriousness>()
                .ForMember(dest => dest.TableMatrixValues, opt => opt.MapFrom(src => src.RiskLevelBySeriousnessAndProbabilities));
            CreateMap<TableMatrixValues, RiskLevelBySeriousnessAndProbability>()
                .ReverseMap();
            CreateMap<ApplicationSeriousness, DataAccessLayer.Database.DataTransferObjects.Seriousness>()
                .ForMember(dest => dest.RiskLevelBySeriousnessAndProbabilities, opt => opt.MapFrom(src => src.TableMatrixValues));
        }

        private void MapMatrixModels() {
            CreateMap<Probability, MatrixProbabilityDTO>();
            CreateMap<RiskLevel, MatrixRiskLevelDTO>();
        }
    }
}
