using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Family.Detail {
    public class FamilyDetailRequest : IRequest<IRequestResponse<FamilyDetailResponse>>{
        public int Id { get; set; }
    }
}
