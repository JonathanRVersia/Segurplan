using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Segurplan.Core.Actions.Plans.PlanManagement.Create.Plane;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Create {
    public class CreateProfile : AutoMapper.Profile {

        public CreateProfile() {

            CreateMap<SafetyPlanPlane, SafetyStudyPlanPlane>();
            CreateMap<SafetyPlanPlane, SafetyStudyPlanPlane>()
               .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<SafetyPlanAdditionalData, SafetyStudyPlanDetails>();

            AddPlanCreateProfiles();

        }

        private void AddPlanCreateProfiles() {
            CreateMap<AddPlanPlaneRequest, SafetyStudyPlanPlane>();
            CreateMap<AddPlanPlaneRequestFiles, SafetyStudyPlanPlaneFile>()
                .ForMember(dest => dest.SafetyStudyPlanPlaneId, opt => opt.MapFrom(src => src.PlaneId));
        }
    }
}
