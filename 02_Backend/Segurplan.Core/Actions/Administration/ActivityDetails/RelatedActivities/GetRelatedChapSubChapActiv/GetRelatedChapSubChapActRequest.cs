using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.GetRelatedChapSubChapActiv {
    public class GetRelatedChapSubChapActRequest : IRequest<IRequestResponse<GetRelatedChapSubChapActResponse>> {
        public int IdRelations { get; set; }
    }
}
