using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.PreventiveMeasures.ModalList {
    public class PreventiveMeasureListRequest : IRequest<IRequestResponse<PreventiveMeasureListResponse>> {
        public IEnumerable<ISpecification<PreventiveMeasureListResponse.ListItem>> Specifications { get; set; }
    }
}
