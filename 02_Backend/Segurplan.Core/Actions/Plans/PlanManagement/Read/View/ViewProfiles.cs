using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.View.PlanesDropdowns;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View {
    public class ViewProfiles : AutoMapper.Profile {

        public ViewProfiles() {

            CreateMap<PlaneFamily, FamiliesPlanesDropdowns>();
        }
    }
}
