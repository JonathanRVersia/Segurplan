using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Create.Plane {
    public class AddPlanPlaneRequest : IRequest<IRequestResponse<AddPlanPlaneResponse>> {

        public int IdSafetyStudyPlan { get; set; }

        public string Description { get; set; }

        public string FamilyName { get; set; }

        public int Position { get; set; }

        public int ModifiedBy { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public List<AddPlanPlaneRequestFiles> Files { get; set; }
    }

    public class AddPlanPlaneRequestFiles {
        public string Name { get; set; }

        public string FileName { get; set; }

        public int PlaneId { get; set; }

        public byte[] Data { get; set; }

        public string DataBaseString { get; set; }

    }
}
