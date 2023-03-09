using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Actions.Administration.PreventiveMeasures.ModalList;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Administration.PreventiveMeasures {
    public class PreventiveMeasuresProfile : AutoMapper.Profile {
        public PreventiveMeasuresProfile() {
            CreateMap<PreventiveMeasure, PreventiveMeasureListResponse.ListItem>();
        }
    }
}
