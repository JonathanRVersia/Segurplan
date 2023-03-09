using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Family.Delete {
    public class DeleteFamilyRequest : IRequest<IRequestResponse<DeleteFamilyResponse>>{
        public int Id { get; set; }
    }
}
