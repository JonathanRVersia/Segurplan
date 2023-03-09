using System.Collections.Generic;
using MediatR;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Seriousness.Save {
    public class SaveSeriousnessWithMatrixRequest : IRequest<IRequestResponse<SaveSeriousnessWithMatrixResponse>> {
        public int UserId { get; set; }
        public ApplicationSeriousness Seriousness { get; set; }
    }
}
