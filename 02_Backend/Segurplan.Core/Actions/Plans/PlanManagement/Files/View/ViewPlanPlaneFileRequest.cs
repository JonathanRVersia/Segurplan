using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Files.View {
    public class ViewPlanPlaneFileRequest : IRequest<IRequestResponse<ViewPlanPlaneFileResponse>> {

        public int? GenericPlaneId { get; set; }
        public bool IsFromGenericBlueprint { get; set; }
        public int CreatedPlanId { get; set; }
    }
}
