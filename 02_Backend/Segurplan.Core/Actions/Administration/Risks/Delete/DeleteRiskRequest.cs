using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Risks.Delete {
    public class DeleteRiskRequest : IRequest<IRequestResponse<DeleteRiskResponse>> {
        public int Id { get; set; }
    }
}
