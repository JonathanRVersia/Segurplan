using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Dropdowns.UpdateAffiliatedDependantData {
    public class UpdateAffiliatedDependantDataRequest : IRequest<IRequestResponse<UpdateAffiliatedDependantDataResponse>> {
        public int BusinessAddressId { get; set; }

    }
}
