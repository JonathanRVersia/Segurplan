using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Family.Save {
    public class SaveFamilyRequest : IRequest<IRequestResponse<SaveFamilyResponse>>{
        public int UserId { get; set; }
        public ApplicationFamily family { get; set; }
    }
}
