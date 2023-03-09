using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Risks.Detail {
    public class RiskDetailRequest : IRequest<IRequestResponse<RiskDetailResponse>>{
        public int Id { get; set; }
    }
}
