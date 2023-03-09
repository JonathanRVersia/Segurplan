using System;
using MediatR;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Risks.Save {
    public class SaveRiskRequest : IRequest<IRequestResponse<SaveRiskResponse>>{
        public int UserId { get; set; }
        public ApplicationRisk risk { get; set; }
    }
}
