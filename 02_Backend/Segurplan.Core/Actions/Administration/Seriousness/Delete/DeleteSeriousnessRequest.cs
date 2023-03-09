using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Seriousness.Delete {
    public class DeleteSeriousnessRequest : IRequest<IRequestResponse<DeleteSeriousnessResponse>> {
        public int Id { get; set; }
    }
}
