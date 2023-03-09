using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.DefaultValues {
    public class GetPlanDetailsDefaultValuesRequest : IRequest<IRequestResponse<GetPlanDetailsDefaultValuesResponse>> {
    }
}
