using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.ChapterActivityList {
    public class ChapterActivityListRequest :  IRequest<IRequestResponse<ChapterActivityListResponse>> {
        public IEnumerable<ISpecification<ChapterActivityListResponse.ListItem>> Specifications { get; set; }
    }
}
