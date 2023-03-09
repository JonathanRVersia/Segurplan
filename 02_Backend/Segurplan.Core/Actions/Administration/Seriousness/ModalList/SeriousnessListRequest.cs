using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.Seriousness.ModalList {
    public class SeriousnessListRequest : IRequest<IRequestResponse<SeriousnessListResponse>> {
        public IEnumerable<ISpecification<SeriousnessListResponse.ListItem>> Specifications { get; set; }
    }
}
