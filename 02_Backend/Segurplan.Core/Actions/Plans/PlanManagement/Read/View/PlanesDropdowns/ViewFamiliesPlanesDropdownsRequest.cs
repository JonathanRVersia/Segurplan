using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View.PlanesDropdowns {
    public class ViewFamiliesPlanesDropdownsRequest : IRequest<IRequestResponse<ViewFamiliesPlanesDropdownsResponse>> {
    }
}
